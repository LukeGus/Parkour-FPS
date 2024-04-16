using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FishNet;
using Newtonsoft.Json;
using UnityEngine;
using FishNet.Transporting.Tugboat;

public class MatchmakingManager : MonoBehaviour
{
    private static readonly HttpClient _httpClient = new HttpClient();
    private static readonly string _matchmakerUrl = "https://test-4fd1fe1a2ca07e.edgegap.net";
    private static readonly string _apiToken = "token b6f27fc9-e522-4a8c-a41c-591c92e90dc1";
    
    public GameObject matchmakngUI;

    public GameObject logo;
    public GameObject quitText;
    public GameObject settingsText;
    public GameObject matchmakeText;
    public GameObject Buttons;
    public GameObject errorText;
    public GameObject matchmakingText;
    public GameObject connnectingText;
    
    public Animator matchmakeAnimator;

    private bool isCoroutineRunning = true;

    private void Start()
    {
        ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;
        _httpClient.BaseAddress = new Uri(_matchmakerUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", _apiToken);
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        matchmakngUI.SetActive(false);
        
        logo.SetActive(true);
        quitText.SetActive(true);
        settingsText.SetActive(true);
        matchmakeText.SetActive(true);
        Buttons.SetActive(true);
        
        errorText.SetActive(false);
        matchmakingText.SetActive(true);
        connnectingText.SetActive(false);
    }

    private IEnumerator GetTicketCoroutine(string ticketId)
    {
        matchmakngUI.SetActive(true);

        errorText.SetActive(false);
        
        while (isCoroutineRunning)
        {
            // Call the GetTicket function with the current ticketID
            bool gotTicket = GetTicket(ticketId);

            // Check if GetTicket returned true, and stop the coroutine if so
            if (gotTicket)
            {
                Debug.Log("Server Found");

                matchmakngUI.SetActive(true);
                
                isCoroutineRunning = false;
            }

            // Wait for 5 seconds before the next iteration
            yield return new WaitForSeconds(5f);
        }
    }

    public void CreateTicket()
    {
        string ticketId = CreateTicketTask().Result;
        Debug.Log($"Ticket created: {ticketId}");

        matchmakngUI.SetActive(true);

        matchmakeAnimator.SetTrigger("Out");

        errorText.SetActive(false);

        StartCoroutine(GetTicketCoroutine(ticketId));
    }

    public bool GetTicket(string ticketId)
    {
        matchmakngUI.SetActive(true);

        errorText.SetActive(false);
        
        string ticketData = GetTicketTask(ticketId).Result;
        if (ticketData != null)
        {
            string serverHost = ExtractServerHost(ticketData);
            if (serverHost != null)
            {
                Debug.Log($"Server Host: {serverHost}");

                matchmakingText.SetActive(false);
                connnectingText.SetActive(true);

                string[] hostParts = serverHost.Split(':');

                if (hostParts.Length == 2)
                {
                    string dnsHost = hostParts[0].Trim();

                    try
                    {
                        // Get IP addresses associated with the URL
                        IPAddress[] addresses = Dns.GetHostAddresses(dnsHost);

                        // Get only the first IP address (assuming there's at least one)
                        string ipAddress = addresses.Length > 0 ? addresses[0].ToString() : "";

                        // Use the parsed host as a ushort
                        if (ushort.TryParse(hostParts[1].Trim(), out ushort host))
                        {
                            Debug.Log($"Connecting to server: {ipAddress}");

                            InstanceFinder.NetworkManager.GetComponent<Tugboat>().SetClientAddress(ipAddress);
                            InstanceFinder.NetworkManager.GetComponent<Tugboat>().SetPort(host);

                            InstanceFinder.NetworkManager.GetComponent<Tugboat>().StartConnection(false);

                            return true;
                        }
                        else
                        {
                            Debug.LogError("Unable to parse the second part as ushort");

                            isCoroutineRunning = false;

                            matchmakngUI.SetActive(false);

                            matchmakeAnimator.SetTrigger("In");

                            errorText.SetActive(true);
                            
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        // Handle any exceptions that may occur during DNS resolution
                        Debug.LogError("Error resolving DNS: " + e.Message);

                        isCoroutineRunning = false;

                        matchmakngUI.SetActive(false);

                        matchmakeAnimator.SetTrigger("In");

                        errorText.SetActive(true);
                    }

                    return false;
                }
                else
                {
                    Debug.LogError("Invalid serverHost format");

                    matchmakngUI.SetActive(false);

                    matchmakeAnimator.SetTrigger("In");

                    isCoroutineRunning = false;

                    errorText.SetActive(true);
                    
                    return false;
                }
            }
            else
            {
                Debug.Log("Server Not Found");

                return false;
            }
        }
        else
        {
            Debug.LogError("Ticket not found");

            matchmakngUI.SetActive(false);

            matchmakeAnimator.SetTrigger("In");
            
            isCoroutineRunning = false;
            
            errorText.SetActive(true);

            return false;
        }
    }

    public void DeleteTicket(string ticketId)
    {
        TicketData? deletedTicket = DeleteTicketTask(ticketId).Result;
        if (deletedTicket != null)
        {
            Debug.Log($"Ticket deleted: {deletedTicket}");
        }
        else
        {
            Debug.LogError("Ticket not found");
        }
    }

    public static async Task<string> CreateTicketTask()
    {
        var dataObject = new
        {
            edgegap_profile_id = "test",
            matchmaking_data = new
            {
                selector_data = new
                {
                    mode = "rank",
                    map = "Dust II",
                },
                filter_data = new
                {
                    elo = 734
                }
            }
        };
        string json = JsonConvert.SerializeObject(dataObject);
        StringContent postData = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync("/v1/tickets", postData).ConfigureAwait(false);
        TicketData? parsedData = null;

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            parsedData = JsonConvert.DeserializeObject<Response<TicketData>>(result).data;
        }

        return parsedData?.Id;
    }

    public static async Task<string> GetTicketTask(string ticketId)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/v1/tickets/{ticketId}").ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return null;
    }

    public static async Task<TicketData?> DeleteTicketTask(string ticketId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"/v1/tickets/{ticketId}").ConfigureAwait(false);
        TicketData? parsedData = null;

        if (response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            try
            {
                parsedData = JsonConvert.DeserializeObject<Response<TicketData>>(result).data;
            }
            catch (JsonException ex)
            {
                Debug.LogError($"Error deserializing JSON: {ex.Message}");
            }
        }

        return parsedData;
    }

    private string ExtractServerHost(string ticketData)
    {
        try
        {
            var assignmentData = JsonConvert.DeserializeObject<Response<AssignmentData>>(ticketData).data;
            return assignmentData.Assignment.ServerHost;
        }
        catch
        {
            return null;
        }
    }

    public struct TicketData
    {
        [JsonProperty("ticket_id")]
        public string Id { get; set; }

        [JsonProperty("assignment")]
        public Assignment Assignment { get; set; }
    }

    public struct AssignmentData
    {
        [JsonProperty("assignment")]
        public Assignment Assignment { get; set; }
    }

    public struct Assignment
    {
        [JsonProperty("server_host")]
        public string ServerHost { get; set; }
    }

    public struct Response<T>
    {
        [JsonProperty("request_id")]
        public string RequestId { get; set; }

        [JsonProperty("data")]
        public T data { get; set; }
    }
}
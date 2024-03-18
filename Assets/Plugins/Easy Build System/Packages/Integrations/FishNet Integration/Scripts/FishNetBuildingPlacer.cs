#if EBS_FISHNET

using UnityEngine;

using EasyBuildSystem.Features.Runtime.Buildings.Placer;
using FishNet.Object;

namespace EasyBuildSystem.Packages.Integrations.FishNet
{
    public class FishNetBuildingPlacer : BuildingPlacer
    {
        FishNetBuildingPlacerReflector m_Reflector;

        public override void Start()
        {
            base.Start();

            m_Reflector = GetComponentInParent<FishNetBuildingPlacerReflector>();

            if (m_Reflector.IsOwner)
            {
                base.Awake();
            }
        }

        public override bool PlacingBuildingPart()
        {
            if (!HasPreview())
            {
                return false;
            }

            if (!CanPlacing)
            {
                return false;
            }

            m_Reflector.CmdPlace(GetSelectedBuildingPart.GetGeneralSettings.Identifier,
                GetCurrentPreview.transform.position, GetCurrentPreview.transform.eulerAngles, Vector3.one);

            if (LastBuildMode == BuildMode.EDIT)
            {
                ChangeBuildMode(BuildMode.EDIT, true);
            }
            else
            {
                CancelPreview();
            }

            if (GetAudioSettings.AudioSource != null)
            {
                if (GetAudioSettings.PlacingAudioClips.Length != 0)
                {
                    GetAudioSettings.AudioSource.PlayOneShot(GetAudioSettings.PlacingAudioClips[Random.Range(0,
                        GetAudioSettings.PlacingAudioClips.Length)]);
                }
            }

            return true;
        }

        public override bool DestroyBuildingPart()
        {
            if (!HasPreview())
            {
                return false;
            }

            if (!CanDestroy)
            {
                return false;
            }

            m_Reflector.CmdDestroy(GetCurrentPreview.gameObject.GetComponent<NetworkObject>());

            if (GetAudioSettings.AudioSource != null)
            {
                if (GetAudioSettings.DestroyAudioClips.Length != 0)
                {
                    GetAudioSettings.AudioSource.PlayOneShot(GetAudioSettings.DestroyAudioClips[Random.Range(0,
                        GetAudioSettings.DestroyAudioClips.Length)]);
                }
            }

            return true;
        }
    }
}
#endif
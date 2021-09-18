using DoubleDash.CodingTools.GenericDatabases;

namespace DoubleDash.ComponentManagement.CinemachineManagement 
{
    public interface ICinemachineVirtualCameraPriorityDatabase 
    {
        public void ResetPriority();
    }

    [System.Serializable]
    public class CinemachineVirtualCameraPriorityDatabase : Database<CinemachineVirtualCameraPriority>, ICinemachineVirtualCameraPriorityDatabase
    {
        //Resets each camera's priority as their base priority
        public void ResetPriority()
        {
            //Get all cameras
            CinemachineVirtualCameraPriority[] cameras = GetAllEntriesAsArray();

            //For each camera, reset that camera's priority
            foreach (CinemachineVirtualCameraPriority camera in cameras) camera.ResetPriority(); 
        }
    }
}


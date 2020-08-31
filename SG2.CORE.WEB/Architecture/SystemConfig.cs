using SG2.CORE.BAL.Managers;
using SG2.CORE.MODAL.DTO.SystemSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SG2.CORE.WEB.Architecture
{
    public class SystemConfig
    {
        private static List<SystemSettingsDTO> singleInstance = null;
        private static readonly object lockObject = new object();
        private SystemConfig()
        {


        }
        public static List<SystemSettingsDTO> GetConfigs
        {
            get
            {
                if (singleInstance == null)
                {
                    lock (lockObject)
                    {
                        if (singleInstance == null)
                        {
                            singleInstance = CommonManager.GetSystemConfigs(); //TODO get the configs from repository 
                        }
                    }
                }
                return singleInstance;
            }
        }

        public static List<SystemSettingsDTO> GetConfigsLatest()
        {


            return CommonManager.GetSystemConfigs(); //TODO get the configs from repository 

        }
        public static List<SystemSettingsDTO> ResetConfig()
        {

            singleInstance = null;
            return GetConfigs;

        }

    }
}
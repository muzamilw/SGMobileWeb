using System;
using System.Web;
using Newtonsoft.Json;
using SG2.CORE.COMMON;

namespace SG2.CORE.BAL.Managers
{
    public class SessionManager
    {
        private bool IsInitialize()
        {
            try
            {
                return HttpContext.Current.Session != null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public object Get(string key)
        {
            try
            {
                return IsExists(key) ? HttpContext.Current.Session[key] : null;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Set(string key, object value)
        {
            try
            {
                if (IsInitialize())
                    HttpContext.Current.Session[key] = value;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void Clear()
        {
            try
            {
                HttpContext.Current.Session[GlobalEnums.SessionConstants.Customer] = null;
                HttpContext.Current.Session.Clear();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private bool IsExists(string key)
        {
            try
            {
                return (HttpContext.Current.Session != null && HttpContext.Current.Session[key] != null);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

    }
}

﻿using RadianceOS.System.Security.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadianceOS.System.Security
{
    public static class UAC
    {
        // The User Account Control popup
        // I'll work on this later

        private static List<UACRequest> UACRequestQueue = new List<UACRequest>();

        public static void RequestUAC(UACRequest request)
        {
            UACRequestQueue.Add(request);
        }

        public class UACRequest
        {
            public Action<UACResult> RequestComplete { get; set; }
            public UACResult Result { get; set; }
            private bool Complete = false;
        }

        public class UACResult
        {
            public bool Success { get; private set; }
        }

        public class ApplicationElevation : UACRequest
        {

        }
        public class ApplicationElevationResult : UACResult
        {

        }

        public class UserElevation : UACRequest
        {
            public Session.UserLevel RequestingUserLevel { get; private set; }
            public UserElevation(Session.UserLevel requestingUserLevel,
                Action<UACResult> complete = null)
            {
                RequestingUserLevel = requestingUserLevel;
                RequestComplete = complete;
                Result = new UserElevationResult();
            }
        }

        public class UserElevationResult : UACResult
        {

        }

        public class UserAdminRequest : UACRequest
        {
            
        }
        public class UserAdminRequestResult : UACResult
        {

        }
    }
}
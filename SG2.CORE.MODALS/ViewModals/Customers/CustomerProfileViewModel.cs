﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SG2.CORE.MODAL.ViewModals.Customers
{
    public class CustomerProfileViewModel
    {
        public CustomerProfileUpdateViewModel CustomerProfileUpdateVM { get; set; }
        public CustomerUpdatePasswordViewModel CustomerUpdatePasswordVM { get; set; }
        public bool IsOptedEducationalEmailSeries { get; set; }
        public bool IsOptedMarketingEmail { get; set; }

    }
}

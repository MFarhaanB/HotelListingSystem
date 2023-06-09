﻿using HotelListingSystem.Engines;
using HotelListingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelListingSystem.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpGet]
        [Route("api/customerapi/userlogin")]
        public IHttpActionResult CheckNumberVaild(string Username, string Password)
        {
            MobileGenericReturn mobileGenericReturn = new MobileGenericReturn();
            try
            {
                var result = CustomerApis.GetUserLoginDetails(Username, Password);
                if (result == null) throw new Exception();
                mobileGenericReturn.ReturntValue = result;
                mobileGenericReturn.StatusCode = "200";
                mobileGenericReturn.Message = "OK";
            }
            catch (Exception)
            {
                mobileGenericReturn.ReturntValue = null;
                mobileGenericReturn.StatusCode = "401";
                mobileGenericReturn.Message = "InternalServerError";
            }
            return Ok(mobileGenericReturn);
        }

        [HttpGet]
        [Route("api/customerapi/gethotelrooms")]
        public IHttpActionResult ReturnAvailableHotelRooms()
        {
            MobileGenericReturn mobileGenericReturn = new MobileGenericReturn();
            try
            {
                (List<Room>, List<Hotel>) result = CustomerApis.ReturnAvailablleHotelsRooms();
                if (result.Item1 == null && result.Item2 == null) throw new Exception();
                mobileGenericReturn.ReturntValue = result;
                mobileGenericReturn.StatusCode = "200";
                mobileGenericReturn.Message = "OK";
            }
            catch (Exception)
            {
                mobileGenericReturn.ReturntValue = null;
                mobileGenericReturn.StatusCode = "401";
                mobileGenericReturn.Message = "InternalServerError";
            }
            return Ok(mobileGenericReturn);
        }
    }
}

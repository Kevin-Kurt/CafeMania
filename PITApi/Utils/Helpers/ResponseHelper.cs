using keener.Models;
using Microsoft.AspNetCore.Mvc;
using PTI.Models;
using System;

namespace PTI.Utils
{
    public class ResponseHelper : ControllerBase
    {

        public IActionResult CreateResponse(ResponseModel response) 
        {
            return response.StatusCode switch
            {
                200 => Ok(String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                500 => StatusCode(500, String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                422 => UnprocessableEntity(String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                400 => StatusCode(400, String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                409 => Conflict(String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                401 => Unauthorized(String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                403 => Forbid(response.Message),
                404 => NotFound(String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
                _ => StatusCode(500, String.IsNullOrEmpty(response.Message) ? response.Content : response.Message),
            };
        }

        
    }
}

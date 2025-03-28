﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using PMS.Data.Entities;

namespace PMS.Web.Models.User;
public class ProfileViewModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Remote(action: "VerifyEmailAvailable", controller: "User", AdditionalFields = nameof(Id))]
    public string Email { get; set; }

    [Required]
    public Role Role { get; set; }

}

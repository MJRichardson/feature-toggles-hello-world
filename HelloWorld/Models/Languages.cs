using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models;

public enum Languages
{
   English,
   [Display(Name="Français")]
   French,
   [Display(Name="Español")]
   Spanish,
}
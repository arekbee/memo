using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace memo.Models
{
    public class RejestracjaModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string nazwa { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string haslo { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("haslo", ErrorMessage = "Hasło i jego potwierdzenie są niezgodne.")]
        public string hasloPowtorka { get; set; }

        [Display(Name = "Akceptacja regulaminu")]
        [Compare("regulamin", ErrorMessage = "Musisz zatwierdzić regulamin")]
        public bool regulamin { get; set; }
    }

    public class LogowanieModel
    {
        [Required]
        [Display(Name = "Nazwa użytkownika")]
        public string nazwa { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string haslo { get; set; }
    }


    public class KokpitModel
    {
        public string login { get; set; }
        public string rola { get; set; }
        public string opcja { get; set; }
    }

    public class Pytanie
    {
        public string pytanie { get; set; }
        public string poprawna_odpowiedz { get; set; }
        public string twoja_odpowiedz { get; set; }
    }


}
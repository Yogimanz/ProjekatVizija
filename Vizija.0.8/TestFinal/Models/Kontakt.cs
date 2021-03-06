//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestFinal.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Kontakt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kontakt()
        {
            this.KontaktMails = new HashSet<KontaktMail>();
            this.KontaktTelefons = new HashSet<KontaktTelefon>();
        }
    

        public int IDOsoba { get; set; }
        public int IDPreduzeca { get; set; }

        [Display(Name = "Ime")]
        [Required(ErrorMessage = "Neophodno popuniti polje za ime.")]
        public string Ime { get; set; }
        [Display(Name = "Prezime")]
        [Required(ErrorMessage = "Neophodno popuniti polje za prezime.")]
        public string Prezime { get; set; }
        [Display(Name = "Radno mesto")]
        [Required(ErrorMessage = "Neophodno popuniti polje za radno mesto.")]
        public string RadnoMesto { get; set; }
    
        public virtual GlavnaTabela GlavnaTabela { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KontaktMail> KontaktMails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KontaktTelefon> KontaktTelefons { get; set; }
    }
}

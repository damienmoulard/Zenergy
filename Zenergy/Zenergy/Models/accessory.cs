//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Zenergy.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class accessory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public accessory()
        {
            this.roomContent = new HashSet<roomContent>();
        }
    
        public int accessoryId { get; set; }
        public string accessoryName { get; set; }
        public Nullable<int> accessoryTotalQuantity { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<roomContent> roomContent { get; set; }
    }
}

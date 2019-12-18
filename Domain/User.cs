using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(75, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(75, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(10, ErrorMessage = "El tamaño para el campo {0} debe ser de {1} caracteres", MinimumLength = 10)]
        [RegularExpression(@"^[\d]{0,10}$", ErrorMessage = "Ingresar 10 dígitos")]
        [Index("User_IdentificationCard_Index", IsUnique = true)]
        [Display(Name = "Cédula")]
        public string IdentificationCard { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        [Index("User_Email_Index", IsUnique = true)]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El tamaño máximo para el campo {0} es {1} caracteres")]
        //[Index("User_NickName_Index", IsUnique = true)]
        [Display(Name = "Apodo")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Birthdate { get; set; }

        [MaxLength(20, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MaxLength(256, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public int UserTypeId { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Fotografia")]
        public string Picture { get; set; }

        [Display(Name = "Equipo")]
        public int FavoriteTeamId { get; set; }

        [Display(Name = "Puntos")]
        public int Points { get; set; }

        [Display(Name = "Verificado")]
        public bool IsVerified { get; set; }

        public string UserASPId { get; set; }

        [NotMapped]
        [Display(Name = "Nombre Completo")]
        public string FullName { get { return string.Format("{0} {1}", LastName, FirstName); } }

        [NotMapped]
        [Display(Name = "Descripción Completa")]
        public string FullDescription { get { return string.Format("{0} {1} - {2} - {3}", LastName, FirstName, Email , IdentificationCard); } }

        public virtual Team FavoriteTeam { get; set; }

        public virtual UserType UserType { get; set; }
        
        public virtual ICollection<Group> UserGroups { get; set; }

        public virtual ICollection<GroupUser> GroupUsers { get; set; }

        public virtual ICollection<Prediction> Predictions { get; set; }

        public virtual ICollection<TeamPlayer> TeamPlayers { get; set; }

        public virtual ICollection<TeamManager> TeamManagers { get; set; }

        public virtual ICollection<NewsItem> News { get; set; }

        public virtual ICollection<LeagueManager> LeagueManagers { get; set; }

        public virtual ICollection<TournamentManager> TournamentManagers { get; set; }

    }

}

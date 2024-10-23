using System;
using System.ComponentModel.DataAnnotations;

namespace makets.Model.Model_users
{
    public partial class DataUser
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно.")]
        public string FirstName { get; set; } = string.Empty;

        public string? Patronymic { get; set; }

        [Required(ErrorMessage = "Дата рождения обязательна.")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Пол обязательный.")]
        public int GenderId { get; set; }

        [Required(ErrorMessage = "Местоположение обязательно.")]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "ID пользователя обязателен.")]
        public int UdrId { get; set; }
    }
}

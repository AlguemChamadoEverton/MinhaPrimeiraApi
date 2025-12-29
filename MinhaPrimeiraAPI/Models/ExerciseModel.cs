using System.ComponentModel.DataAnnotations;

namespace MinhaPrimeiraAPI.Models
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public List<ExerciseMuscleModel> ExerciseMuscles { get; } = [];
    }
    //Buguei a mente aqui, não sei como fazer a relação no código que está dando erro, aqui antes tinha uma ligação do Muscle com o exercise diretamente aqui, mas eu dispensei isso,
    //talvez eu precise, mas porque precisaria se só importa diretamente na tabela de ligação, preciso pensar um jeito de colocar isso direto na tabela de ligação ou sei lá o que
}

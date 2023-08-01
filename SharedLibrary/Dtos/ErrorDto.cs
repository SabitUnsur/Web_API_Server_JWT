namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; } = new List<string>();
        public bool IsShow { get; private set; }
        //Burası kullancıya dönülecek hataları ayarlar.yazılımcıya dönecek hataları kullanıcıya göstermemek için bir belirteçtir 

        public ErrorDto(string error, bool isShow)
        {
            Errors.Add(error);
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }

    }
}

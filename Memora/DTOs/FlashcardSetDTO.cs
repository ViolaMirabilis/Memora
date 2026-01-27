namespace Memora.DTOs;

public class FlashcardSetDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? FolderId { get; set; }
    public string? FolderName { get; set; } = "NULL";

    public override string ToString()
    {
        return this.Id + "\t" + this.Name + "\t" + this.FolderId + "\t" + this.FolderName;
    }
}

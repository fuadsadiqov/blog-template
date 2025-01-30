namespace GP.Core.Configurations.Entity
{
    public interface ITreeEntity<T>
    {
        public List<T> Children { get; set; }
    }
}

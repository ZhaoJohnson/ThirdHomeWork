namespace ThirdWorkModel.CommonModel
{
    public class StoryModel
    {
        public string HeroPosition { get; set; }
        public string[] LevelUpStory { get; set; }
    }

    public class FullStoryModel
    {
        public StoryModel[] MyFullStory { get; set; }
    }
}
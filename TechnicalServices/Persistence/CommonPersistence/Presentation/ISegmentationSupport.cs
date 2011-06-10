namespace TechnicalServices.Persistence.CommonPersistence.Presentation
{
    public interface ISegmentationSupport
    {
        int SegmentRows { get; }
        int SegmentColumns { get; }
        int SegmentWidth { get; }
        int SegmentHeight { get; }
    }
}

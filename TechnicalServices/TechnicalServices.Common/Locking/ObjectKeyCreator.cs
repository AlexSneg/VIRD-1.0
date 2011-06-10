using TechnicalServices.Entity;
using TechnicalServices.Persistence.SystemPersistence.Presentation;

namespace TechnicalServices.Common.Locking
{
    public static class ObjectKeyCreator
    {
        public static PresentationKey CreatePresentationKey(string presentationUniqueName)
        {
            return new PresentationKey(presentationUniqueName);
        }

        public static PresentationKey CreatePresentationKey(Presentation presentation)
        {
            return new PresentationKey(presentation.UniqueName);
        }

        public static PresentationKey CreatePresentationKey(PresentationInfo presentationInfo)
        {
            return new PresentationKey(presentationInfo.UniqueName);
        }

        public static SlideKey CreateSlideKey(string presentationUniqueName, int slideId)
        {
            return new SlideKey(presentationUniqueName, slideId);
        }

        public static SlideKey CreateSlideKey(PresentationKey presentationKey, int slideId)
        {
            return new SlideKey(presentationKey, slideId);
        }

        public static SlideKey CreateSlideKey(Presentation presentation, Slide slide)
        {
            return new SlideKey(presentation.UniqueName, slide.Id);
        }

    }
}

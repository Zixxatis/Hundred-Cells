using System.Collections.Generic;
using System.Linq;

namespace CGames
{
    public class TutorialsRSS
    {
        private readonly List<TutorialTopicSO> tutorialTopicsList;

        public TutorialsRSS(List<TutorialTopicSO> tutorialTopicsList)
        {
            this.tutorialTopicsList = tutorialTopicsList;
        }

        public TutorialTopicSO GetTutorialTopicSO(TutorialTopicTheme tutorialTopicTheme)
        {
            return tutorialTopicsList.First(x => x.TutorialTopicTheme == tutorialTopicTheme);
        }
    }
}
using InContextLevelEditor.LevelEditor;

namespace InContextLevelEditor.Strategy
{
    public abstract class Strategy 
    {
        protected IEntity entity;

        protected Strategy(IEntity entity)
        {
            this.entity = entity;
        }

        public abstract void Execute();
    }
}
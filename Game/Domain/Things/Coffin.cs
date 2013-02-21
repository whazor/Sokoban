namespace Sokoban.Domain.Domain.Things
{
    class Coffin : IThing
    {
        public string ResourceName
        {
            get
            {
                return OnDestination ? "coffin_ok" : "coffin";
            }
        }

        public bool OnDestination { get; set; }
    }
}

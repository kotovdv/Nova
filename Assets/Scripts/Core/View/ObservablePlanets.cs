using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Model.Space;
using Core.Model.Space.Grid;

namespace Core.View
{
    public class ObservablePlanets
    {
        private readonly SpaceGrid _spaceGrid;
        private readonly AltObservableSet _altObservableSet;
        private readonly IDictionary<Position, Planet> _observable;
        private readonly IDictionary<Position, Planet> _altObservable;
        private readonly ReadOnlyDictionary<Position, Planet> _readOnlyObservable;
        private readonly ReadOnlyDictionary<Position, Planet> _readOnlyAltObservable;

        public readonly IPlanetAction Show;
        public readonly IPlanetAction Hide;
        public readonly IPlanetAction AltShow;
        public readonly IPlanetAction AltHide;
        public readonly IPlanetAction CompositeShow;

        public ObservablePlanets(SpaceGrid spaceGrid, int altViewCapacity, int playerRating)
        {
            _spaceGrid = spaceGrid;
            _observable = new Dictionary<Position, Planet>();
            _altObservable = new Dictionary<Position, Planet>();
            _readOnlyObservable = new ReadOnlyDictionary<Position, Planet>(_observable);
            _readOnlyAltObservable = new ReadOnlyDictionary<Position, Planet>(_altObservable);
            _altObservableSet = new AltObservableSet(altViewCapacity, playerRating);

            Show = new ShowAction(this);
            Hide = new HideAction(this);
            AltShow = new AltShowAction(this);
            AltHide = new AltHideAction(this);
            CompositeShow = new CompositePlanetAction(this, new[] {Show, AltShow});
        }

        public ReadOnlyDictionary<Position, Planet> GetObservablePlanets()
        {
            return _readOnlyObservable;
        }

        public ReadOnlyDictionary<Position, Planet> GetAltObservablePlanets()
        {
            _altObservable.Clear();
            var currentlyVisible = _altObservableSet.CurrentlyVisible();
            foreach (var position in currentlyVisible)
            {
                var planet = _spaceGrid.GetPlanet(position);
                _altObservable[position] = planet;
            }

            return _readOnlyAltObservable;
        }

        private abstract class PlanetAction : IPlanetAction
        {
            protected readonly ObservablePlanets Planets;

            protected PlanetAction(ObservablePlanets planets)
            {
                Planets = planets;
            }

            public abstract void Invoke(Position position, Planet planet);
        }

        private class CompositePlanetAction : PlanetAction
        {
            private readonly IPlanetAction[] _actions;

            public CompositePlanetAction(ObservablePlanets planets, IPlanetAction[] actions) : base(planets)
            {
                _actions = actions;
            }

            public override void Invoke(Position position, Planet planet)
            {
                for (var i = 0; i < _actions.Length; i++)
                {
                    _actions[i].Invoke(position, planet);
                }
            }
        }

        private class ShowAction : PlanetAction
        {
            public ShowAction(ObservablePlanets planets) : base(planets)
            {
            }

            public override void Invoke(Position position, Planet planet)
            {
                Planets._observable[position] = planet;
            }
        }

        private class HideAction : PlanetAction
        {
            public HideAction(ObservablePlanets planets) : base(planets)
            {
            }

            public override void Invoke(Position position, Planet planet)
            {
                Planets._observable.Remove(position);
            }
        }

        private class AltShowAction : PlanetAction
        {
            public AltShowAction(ObservablePlanets planets) : base(planets)
            {
            }

            public override void Invoke(Position position, Planet planet)
            {
                Planets._altObservableSet.Add(position, planet.Rating);
            }
        }

        private class AltHideAction : PlanetAction
        {
            public AltHideAction(ObservablePlanets planets) : base(planets)
            {
            }

            public override void Invoke(Position position, Planet planet)
            {
                Planets._altObservableSet.Remove(position, planet.Rating);
            }
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Model.Space;

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

        public ObservablePlanets(SpaceGrid spaceGrid, int altViewCapacity, int playerRating)
        {
            _spaceGrid = spaceGrid;
            _observable = new Dictionary<Position, Planet>();
            _altObservable = new Dictionary<Position, Planet>();
            _readOnlyObservable = new ReadOnlyDictionary<Position, Planet>(_observable);
            _readOnlyAltObservable = new ReadOnlyDictionary<Position, Planet>(_altObservable);
            _altObservableSet = new AltObservableSet(altViewCapacity, playerRating);
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

        public void CombinedShow(Position position, Planet planet)
        {
            Show(position, planet);
            AltShow(position, planet);
        }

        public void CombinedHide(Position position, Planet planet)
        {
            Hide(position);
            AltHide(position, planet);
        }

        public void Show(Position position, Planet planet)
        {
            _observable[position] = planet;
        }

        public void AltShow(Position position, Planet planet)
        {
            _altObservableSet.Add(position, planet.Rating);
        }

        public void Hide(Position position)
        {
            _observable.Remove(position);
        }

        public void AltHide(Position position, Planet planet)
        {
            _altObservableSet.Remove(position, planet.Rating);
        }
    }
}
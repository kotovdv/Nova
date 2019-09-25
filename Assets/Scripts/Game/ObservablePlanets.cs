using System.Collections.Generic;
using System.Collections.ObjectModel;

public partial class Game
{
    private readonly AlternativeViewSet _alternativeViewSet;
    private readonly IDictionary<Position, Planet> _observable;
    private readonly ReadOnlyDictionary<Position, Planet> _readOnlyObservable;
    private readonly IDictionary<Position, Planet> _altObservable;
    private readonly ReadOnlyDictionary<Position, Planet> _readOnlyAltObservable;

    private void CombinedShow(Position position)
    {
        Show(position);
        AltShow(position);
    }

    private void CombinedHide(Position position)
    {
        Hide(position);
        AltHide(position);
    }

    private void Show(Position position)
    {
        _observable[position] = _spaceGrid.GetPlanet(position);
    }

    private void AltShow(Position position)
    {
        var planet = _spaceGrid.GetPlanet(position);

        _alternativeViewSet.Add(position, planet.Rating);
    }

    private void Hide(Position position)
    {
        _observable.Remove(position);
    }

    private void AltHide(Position position)
    {
        _alternativeViewSet.Remove(position, _spaceGrid.GetPlanet(position).Rating);
    }
}
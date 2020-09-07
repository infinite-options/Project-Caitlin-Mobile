using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjectCaitlin.ViewModel
{
    public class TodaysListViewModel
    {
        WeakReference<TodaysList> MainPageWeakReference;
        INavigation Navigation;
        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();
        private WeakReference<object> weakReference;

        public TodaysListViewModel(INavigation navigation, WeakReference<TodaysList> MainPageWeakReference)
        {
            this.Navigation = navigation;
            this.MainPageWeakReference = MainPageWeakReference;
        }

        async public void OnTileTapped(TodaysListTileDisplayObject Tile)
        {
            //MainPageWeakReference.TryGetTarget(out TodaysList mainPage);
            if (Tile.Type== TileType.Goal)
            {
                HandleGoalTileTouch(Tile);
            }
            else if(Tile.Type == TileType.Routine)
            {
                HandleRoutineTileTouch(Tile);
            }
        }

        async public void UponTileUpdate(TodaysListTileDisplayObject Tile)
        {
            if (!Tile.InProgress && !Tile.IsComplete)
            {
                Tile.InProgress = true;
                await firebaseFunctionsService.updateGratisStatus(Tile.GratisObject as GratisObject, "goals&routines", false);
            }
        }

        async void HandleGoalTileTouch(TodaysListTileDisplayObject Tile)
        {
            goal goal = Tile.GratisObject as goal;
            if (!goal.isSublistAvailable || goal.actions.Count==0)
            {
                UponTileUpdate(Tile);
            }
            else
            {
                Navigation.PushAsync(new TaskCompletePageCopy(Tile.Index, false, null, async () => { UponTileUpdate(Tile); }));
            }
        }

        async void HandleRoutineTileTouch(TodaysListTileDisplayObject Tile)
        {
            routine routine = Tile.GratisObject as routine;
            if (!routine.isSublistAvailable || routine.tasks.Count == 0)
            {
                UponTileUpdate(Tile);
            }
            else
            {
                Navigation.PushAsync(new StepsPageCopy(Tile.Index, true, Tile.GratisObject as GRItemModel, async () => { UponTileUpdate(Tile); }));
            }
        }

    }
}

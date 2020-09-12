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

        async public void UponGoalTileUpdate(TodaysListTileDisplayObject Tile)
        {
            if (!Tile.InProgress && !Tile.IsComplete)
            {
                Tile.InProgress = true;
                await firebaseFunctionsService.updateGratisStatus(App.User.goals[Tile.Index], "goals&routines", false);
            }
        }

        async public void CompleteGoal(TodaysListTileDisplayObject Tile)
        {
            Tile.InProgress = false;
            Tile.IsComplete = true;
            await firebaseFunctionsService.updateGratisStatus(App.User.goals[Tile.Index], "goals&routines", true);
        }


        async void  CompleteRoutineUI(TodaysListTileDisplayObject Tile)
        {
            Tile.IsComplete = true;
        }

        async public void CompleteRoutine(TodaysListTileDisplayObject Tile)
        {
            Tile.InProgress = false;
            Tile.IsComplete = true;
            await firebaseFunctionsService.updateGratisStatus(App.User.routines[Tile.Index], "goals&routines", true);
        }

        async public void UponRoutineTileUpdate(TodaysListTileDisplayObject Tile)
        {
            if (!Tile.InProgress && !Tile.IsComplete)
            {
                Tile.InProgress = true;
                await firebaseFunctionsService.updateGratisStatus(App.User.routines[Tile.Index], "goals&routines", false);
            }
        }

        async void HandleGoalTileTouch(TodaysListTileDisplayObject Tile)
        {
            goal goal = App.User.goals[Tile.Index];
            if (!goal.isSublistAvailable || goal.actions.Count==0)
            {
                if (Tile.InProgress)
                {
                    CompleteGoal(Tile);
                } else 
                {
                    UponGoalTileUpdate(Tile); 
                }
            }
            else
            {
                await Navigation.PushAsync(new TaskCompletePageCopy(Tile.Index, false, null, async () => { UponGoalTileUpdate(Tile); }, null, async () => { CompleteGoal(Tile); }));
            }
        }


        

        async void HandleRoutineTileTouch(TodaysListTileDisplayObject Tile)
        {
            routine routine = App.User.routines[Tile.Index];
            if (!routine.isSublistAvailable || routine.tasks.Count == 0)
            {
                if (Tile.InProgress)
                {
                    CompleteRoutine(Tile);
                }
                else
                {
                    UponRoutineTileUpdate(Tile);
                }
            }
            else
            {
                await Navigation.PushAsync(new StepsPageCopy(Tile.Index, true, Tile.GratisObject as GRItemModel, async () => { UponRoutineTileUpdate(Tile); }, async () => { CompleteRoutineUI(Tile); }));
            }
        }

    }
}

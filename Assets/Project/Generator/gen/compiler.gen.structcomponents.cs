namespace ME.ECS {

    public static partial class ComponentsInitializer {

        static partial void InitTypeIdPartial() {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.CollisionWithEntity>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.BoardSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.Food>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.PositionOnBoard>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.SpawnFood>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ToDespawnTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.DestroyOverTime.Components.TimeToDestroy>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.EatenApples>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrepareToMoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrevPositionInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeLink>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.StartMovePosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.TargetPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ChangePositionEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.DestroyOverTime.Components.ToDespawn>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsMove>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakeHead>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SpawnSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.DataConfigViewReference>(false, true, false, false, false, false, false, false, false);

        }

        static partial void Init(State state, ref ME.ECS.World.NoState noState) {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.CollisionWithEntity>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.BoardSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.Food>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.PositionOnBoard>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.SpawnFood>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ToDespawnTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.DestroyOverTime.Components.TimeToDestroy>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.EatenApples>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrepareToMoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrevPositionInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeLink>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.StartMovePosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.TargetPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ChangePositionEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.DestroyOverTime.Components.ToDespawn>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsMove>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakeHead>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SpawnSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.DataConfigViewReference>(false, true, false, false, false, false, false, false, false);

            ComponentsInitializerWorld.Setup(ComponentsInitializerWorldGen.Init);
            CoreComponentsInitializer.Init(state, ref noState);


            state.structComponents.ValidateUnmanaged<Project.Components.CollisionWithEntity>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.BoardSize>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.Food>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.PositionOnBoard>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.SpawnFood>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.ToDespawnTime>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.DestroyOverTime.Components.TimeToDestroy>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.EatenApples>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.MoveDirection>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.MoveTime>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.PrepareToMoveTime>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.PrevPositionInfo>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeLink>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeStartPosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeStartSize>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.StartMovePosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.TargetPosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.ChangePositionEvent>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.DestroyOverTime.Components.ToDespawn>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsMove>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsSnakeHead>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsSnakePart>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakePartsUpdateEvent>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SpawnSnakePart>(ref state.allocator, true);
            state.structComponents.Validate<Project.Components.DataConfigViewReference>(false);

        }

    }

    public static class ComponentsInitializerWorldGen {

        public static void Init(Entity entity) {


            entity.ValidateDataUnmanaged<Project.Components.CollisionWithEntity>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.BoardSize>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.Food>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.PositionOnBoard>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.SpawnFood>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.ToDespawnTime>(false);
            entity.ValidateDataUnmanaged<Project.Features.DestroyOverTime.Components.TimeToDestroy>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.EatenApples>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.MoveDirection>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.MoveTime>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.PrepareToMoveTime>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.PrevPositionInfo>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeLink>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeStartPosition>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeStartSize>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.StartMovePosition>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.TargetPosition>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.ChangePositionEvent>(true);
            entity.ValidateDataUnmanaged<Project.Features.DestroyOverTime.Components.ToDespawn>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsMove>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsSnakeHead>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsSnakePart>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SpawnSnakePart>(true);
            entity.ValidateData<Project.Components.DataConfigViewReference>(false);

        }

    }

}

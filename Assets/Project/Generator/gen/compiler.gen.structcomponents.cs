namespace ME.ECS {

    public static partial class ComponentsInitializer {

        static partial void InitTypeIdPartial() {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.CollisionWithEntity>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameEndInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.BoardSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.Food>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.PositionOnBoard>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.SpawnFoodEvent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Destroy.Components.TimeToDestroy>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrepareToMoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrevPositionInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeLink>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.StartMovePosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.TargetPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GamePaused>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ChangePositionEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Destroy.Components.ToDespawn>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsInitPart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsMove>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakeHead>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SpawnSnakePartEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.DataConfigViewReference>(false, true, false, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.ParticleEffects.Components.OnDespawnParticle>(false, true, false, false, false, false, false, false, false);

        }

        static partial void Init(State state, ref ME.ECS.World.NoState noState) {

            WorldUtilities.ResetTypeIds();

            CoreComponentsInitializer.InitTypeId();


            WorldUtilities.InitComponentTypeId<Project.Components.CollisionWithEntity>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameEndInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GameInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.BoardSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.Food>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.PositionOnBoard>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.SpawnFoodEvent>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Destroy.Components.TimeToDestroy>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveDirection>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.MoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrepareToMoveTime>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.PrevPositionInfo>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeLink>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakeStartSize>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.StartMovePosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.TargetPosition>(false, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.GamePaused>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Board.Components.ChangePositionEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Destroy.Components.ToDespawn>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsInitPart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsMove>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakeHead>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.IsSnakePart>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.Snake.Components.SpawnSnakePartEvent>(true, true, true, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Components.DataConfigViewReference>(false, true, false, false, false, false, false, false, false);
            WorldUtilities.InitComponentTypeId<Project.Features.ParticleEffects.Components.OnDespawnParticle>(false, true, false, false, false, false, false, false, false);

            ComponentsInitializerWorld.Setup(ComponentsInitializerWorldGen.Init);
            CoreComponentsInitializer.Init(state, ref noState);


            state.structComponents.ValidateUnmanaged<Project.Components.CollisionWithEntity>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Components.GameEndInfo>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Components.GameInfo>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.BoardSize>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.Food>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.PositionOnBoard>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.SpawnFoodEvent>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Destroy.Components.TimeToDestroy>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.MoveDirection>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.MoveTime>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.PrepareToMoveTime>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.PrevPositionInfo>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeLink>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeStartPosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakeStartSize>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.StartMovePosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.TargetPosition>(ref state.allocator, false);
            state.structComponents.ValidateUnmanaged<Project.Components.GamePaused>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Board.Components.ChangePositionEvent>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Destroy.Components.ToDespawn>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsInitPart>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsMove>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsSnakeHead>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.IsSnakePart>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SnakePartsUpdateEvent>(ref state.allocator, true);
            state.structComponents.ValidateUnmanaged<Project.Features.Snake.Components.SpawnSnakePartEvent>(ref state.allocator, true);
            state.structComponents.Validate<Project.Components.DataConfigViewReference>(false);
            state.structComponents.Validate<Project.Features.ParticleEffects.Components.OnDespawnParticle>(false);

        }

    }

    public static class ComponentsInitializerWorldGen {

        public static void Init(Entity entity) {


            entity.ValidateDataUnmanaged<Project.Components.CollisionWithEntity>(false);
            entity.ValidateDataUnmanaged<Project.Components.GameEndInfo>(false);
            entity.ValidateDataUnmanaged<Project.Components.GameInfo>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.BoardSize>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.Food>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.PositionOnBoard>(false);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.SpawnFoodEvent>(false);
            entity.ValidateDataUnmanaged<Project.Features.Destroy.Components.TimeToDestroy>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.MoveDirection>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.MoveTime>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.PrepareToMoveTime>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.PrevPositionInfo>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeLink>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeStartPosition>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakeStartSize>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.StartMovePosition>(false);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.TargetPosition>(false);
            entity.ValidateDataUnmanaged<Project.Components.GamePaused>(true);
            entity.ValidateDataUnmanaged<Project.Features.Board.Components.ChangePositionEvent>(true);
            entity.ValidateDataUnmanaged<Project.Features.Destroy.Components.ToDespawn>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsInitPart>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsMove>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsSnakeHead>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.IsSnakePart>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SnakePartsUpdateEvent>(true);
            entity.ValidateDataUnmanaged<Project.Features.Snake.Components.SpawnSnakePartEvent>(true);
            entity.ValidateData<Project.Components.DataConfigViewReference>(false);
            entity.ValidateData<Project.Features.ParticleEffects.Components.OnDespawnParticle>(false);

        }

    }

}

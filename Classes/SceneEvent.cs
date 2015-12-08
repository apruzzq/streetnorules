using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Les_Loubards
{
    class SceneEvent
    {
        Trigger triggerToAdd;
        int addToPlayBounds;
        List<EnemySpawner> enemySpawnersToAdd;
        List<Actor> enemiesToAdd;

        public SceneEvent()
        {
            triggerToAdd = null;
            addToPlayBounds = 0;
            enemySpawnersToAdd = new List<EnemySpawner>();
            enemiesToAdd = new List<Actor>();
        }

        public void AddTrigger(Trigger trigger)
        {
            triggerToAdd = trigger;
        }

        public void AddPlayBounds(int howMuch)
        {
            this.addToPlayBounds = howMuch;
        }

        public void AddEnemySpawners(EnemySpawner spawner)
        {
            this.enemySpawnersToAdd.Add(spawner);
        }

        public void AddEnemy(Actor enemy)
        {
            this.enemiesToAdd.Add(enemy);
        }

        public virtual void Activate(Level level)
        {
            //Add in the Trigger to the level
            level.CurrentTrigger = this.triggerToAdd;

            //Add to the PlayBounds
            level.AddToPlayBounds(this.addToPlayBounds);

            //Add the EnnemySpawner to the level
            for (int i = 0; i < enemySpawnersToAdd.Count; i++)
            {
                level.EnemySpawners.Add(enemySpawnersToAdd[i]);
            }

            foreach (Actor enemy in enemiesToAdd)
            {
                Level.GetStarttSidePosition(enemy, level);
                level.Actors.Add(enemy);
            }
        }
    }

    class SceneEventActivateEnemies : SceneEvent
    {
        public SceneEventActivateEnemies()
            : base() { }

        
        public override void Activate(Level level)
        {
            // Make all the "Waiting" enemies start fighting the player
            for (int i = 0; i < level.Actors.Count; i++)
            {
                EnemyCloseQuentin enemy = level.Actors[i] as EnemyCloseQuentin;

                if (enemy != null)
                {
                    enemy.ResetIdleGraphic();
                }
            }
            for (int i = 0; i < level.Actors.Count; i++)
            {
                EnemyCloseMehdi enemy = level.Actors[i] as EnemyCloseMehdi;

                if (enemy != null)
                {
                    enemy.ResetIdleGraphic();
                }
            }

            base.Activate(level);
        }
    }
}

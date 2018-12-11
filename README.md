# Unity-MLAgents-stuff

## FollowMe (RollerBall) Game

This environment is based on [Making a New Learning Environment](https://github.com/Unity-Technologies/ml-agents/blob/master/docs/Learning-Environment-Create-New.md). A simple Roll-a-Ball game where the objective is to collect a box by controlling a ball. The player should avoid falling off the edge.

In order to speed up the training, I created 12 copies of the environment in the same scene with a script. This allows learning from 13 agents simultaneously each frame during training. [Check out the script here.](./FollowMe/Assets/Scripts/EnvironmentCloner.cs)

### Modification made in the scripts to improve the training process
The changes were made in the policy of giving reward. Earlier the system will get +1 reward if the distance between ball and target is less than < 1.42 and negative reward -1 if the ball falls from the plane while the time penalty was -0.5 but, it was not learning well. So, I thought of changing the reward policy. I gave minor reward of +0.04 if the ball is near to the cube (target) by comparing the distance of the ball from target in each frame with the distance of the ball from target from previous frame. I reduced the time penalty to -0.05. Thus, in theory if the ball remains on the plane and if ball is closer to target from its previous frame it will still get negative reward of -0.01. By doing this, I motivated the ball to reach to the target as quickly as possible.

One of the parameters from Training config were overridden for this experiment. The `max_steps` was increased to 5.0e6 to train for a much longer time.


### Results

![Alt](./FollowMe/images/playtest.gif "Record of agent performance after final training")

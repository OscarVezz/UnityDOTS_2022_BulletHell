using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    namespace Events
    {
        [System.Serializable]
        public class Trigger
        {
            public int Id;
            public float Start;
        }


        [System.Serializable]
        public class TriggerList
        {
            public Trigger[] triggers;
        }


        [System.Serializable]
        public class SpawnersInformation
        {
            public GameObject go;
            public Vector3 position;
            public Vector3 right;

            public bool react;
            public float shutDownTime;
            public float nextSpawnTime;

            [Range(0f, 1f)]
            public float spawnArc;
            public float spawnRate;

            public float projectileSpeed;
            public float projectileLifetime;
        }


        [System.Serializable]
        public class SpawnerReactStep
        {
            public int step;

            public bool basicBitch;

            public float spawnArc;
            public float spawnRate;
            public float maxSpawnTime;

            public float projectileSpeed;
            public float projectileLifetime;

            public SpawnerReactStep()
            {
                step = 0;

                basicBitch = true;

                spawnArc = 0f;
                spawnRate = 0.01f;
                maxSpawnTime = 0.5f;

                projectileSpeed = 10f;
                projectileLifetime = 1f;
            }

            public SpawnerReactStep(int step, float spawnArc, float spawnRate, float maxSpawnTime, float projectileSpeed, float projectileLifetime)
            {
                basicBitch = true;

                this.step = step;

                this.spawnArc = spawnArc;
                this.spawnRate = spawnRate;
                this.maxSpawnTime = maxSpawnTime;

                this.projectileSpeed = projectileSpeed;
                this.projectileLifetime = projectileLifetime;
            }
        }

        [System.Serializable]
        public class MovableReactStep
        {
            public int step;

            public bool basicBitch;

            public bool comeBack;
            public Vector3 vectorChange;
            public float startSpeed;
            public float returnSpeed;

            public MovableReactStep()
            {
                step= 0;

                basicBitch = true;

                comeBack = true;
                vectorChange = Vector3.zero;
                startSpeed = 1;
                returnSpeed = 1;
            }

            public MovableReactStep(int step, bool comeBack, Vector3 vectorChange, float startSpeed, float returnSpeed)
            {
                basicBitch = true;

                this.step = step;
                this.comeBack = comeBack;
                this.vectorChange = vectorChange;
                this.startSpeed = startSpeed;
                this.returnSpeed = returnSpeed;
            }
        }

        [System.Serializable]
        public class RotableReactStep
        {
            public int step;

            public bool chasePlayer;

            public bool comeBack;
            public float yChange;
            public float startSpeed;
            public float returnSpeed;
            public float offset;

            public Easing.EasingType easingType;

            public RotableReactStep(int step)
            {
                this.step = step;

                chasePlayer = false;

                comeBack = false;
                yChange = 0;
                startSpeed = 1;
                returnSpeed = 1;
                offset = 0;

                easingType = Easing.EasingType.Linear;
            }
        }
    }

    namespace Easing
    {
        public enum EasingType
        {
            Linear,
            InCubic,
            OutCubic,
            InExpo,
            OutExpo,
            InElastic,
            OutElastic,
            InQuad,
            OutQuad
        }

        public static class MathEasing
        {
            public static float Evaluate(float x, EasingType type)
            {


                switch (type)
                {
                    case EasingType.Linear:
                        {
                            return x;
                        }

                    case EasingType.InCubic:
                        {
                            return x * x * x;
                        }

                    case EasingType.OutCubic:
                        {
                            return 1 - Mathf.Pow(1 - x, 3);
                        }

                    case EasingType.InExpo:
                        {
                            return x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10);
                        }

                    case EasingType.OutExpo:
                        {
                            return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
                        }

                    case EasingType.InElastic:
                        {
                            float c4 = (2 * Mathf.PI) / 3;
                            return x == 0
                              ? 0
                              : x == 1
                              ? 1
                              : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10.0f - 10.75f) * c4);
                        }

                    case EasingType.OutElastic:
                        {
                            float c4 = (2 * Mathf.PI) / 3;
                            return x == 0
                              ? 0
                              : x == 1
                              ? 1
                              : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10.0f - 0.75f) * c4) + 1;
                        }

                    case EasingType.InQuad:
                        {
                            return x * x;
                        }

                    case EasingType.OutQuad:
                        {
                            return 1 - (1 - x) * (1 - x);
                        }
                }

                

                return x;
            }
        }
    }

    namespace Audio
    {
        public enum SoundType
        {
            Main,
            Attack
        }

        [System.Serializable]
        public class Sound
        {
            public SoundType type;

            public AudioClip clip;

            [Range(0f, 1f)]
            public float volume;
            [Range(0.1f, 3f)]
            public float pitch;

            [HideInInspector]
            public AudioSource source;
        }
    }
}

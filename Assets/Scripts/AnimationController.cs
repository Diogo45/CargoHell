using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CargoHell.Animation
{
    public class AnimationController : Singleton<AnimationController>
    {
        [Flags]
        public enum AnimStates
        {
            None = 0, PlayerCentering = 1, PlayerStretch = 2, PlayerGoToInfinityAndBeyond = 4, NebulaTilling = 8, NebulaSpeed = 16
        }

        public AnimStates animationState = AnimStates.PlayerCentering;

        public GameObject explosionAnim;

        public Material nebulaMat;
        public GameObject Stars;

        public AnimationCurve tillingCurve;
        public AnimationCurve scrollSpeedCurve;


        private Vector4 nebula;
        private Vector4 nebulaScrollSpeed;

        public float frame { get; private set; }

        public GameObject _player { get; private set; }
        public GameObject _shield { get; private set; }

        [field: SerializeField]
        public float AnimSpeed { get; private set; }



        // Start is called before the first frame update
        void Start()
        {
            _player = LevelController.instance.Player;
            _shield = LevelController.instance.Shield;


            var _level = LevelController.instance._level;

            nebulaMat.SetColor("_Color", new Color(_level.ShaderVariables.color.r, _level.ShaderVariables.color.g, _level.ShaderVariables.color.b));
            nebulaMat.SetVector("_Tilling", new Vector4(1, 1, 1, 1));
            nebulaMat.SetVector("_ScrollSpeed", new Vector4(0.05f, 0.05f, 1, 1));


            nebula = nebulaMat.GetVector("_Tilling");
            nebulaScrollSpeed = nebulaMat.GetVector("_ScrollSpeed");

        }

        // Update is called once per frame
        void Update()
        {



            //if (Camera.main.WorldToViewportPoint(_player.transform.position).x > 1.2f)
            //{
            //    animationState = AnimStates.None;
            //    WinCanvas.SetActive(true);
            //    var score = Score.ToString().ToCharArray();

            //    intCount = new int[score.Length];
            //    finishedCount = new bool[score.Length];
            //    StartCoroutine(countScore());



            //    for (int i = 0; i < score.Length; i++)
            //    {
            //        StartCoroutine(CountUp(i));
            //    }
            //    //AuxAudioSource.clip = scoreCountAudio;
            //    //AuxAudioSource.loop = true;
            //    //AuxAudioSource.Play();

            //}



            if (animationState == AnimStates.PlayerGoToInfinityAndBeyond)
            {
                //AuxAudioSource.Stop();
                AnimateNebula();
            }

            



        }

        public void AnimateNebula()
        {
          

            var nebulaFinal = new Vector4(nebula.x, 500f, nebula.z, nebula.w);
            var nebulaFinalScrollSpeed = new Vector4(30f, 0f, nebulaScrollSpeed.z, nebulaScrollSpeed.w);

            var tillingSpeed = tillingCurve.Evaluate(frame);
            var scrollSpeed = scrollSpeedCurve.Evaluate(frame);

            nebulaMat.SetVector("_Tilling", Vector4.Lerp(nebula, nebulaFinal, tillingSpeed));
            nebulaMat.SetVector("_ScrollSpeed", Vector4.Lerp(nebulaScrollSpeed, nebulaFinalScrollSpeed, scrollSpeed));

            frame += Time.deltaTime * AnimSpeed;
        }

        public void Explosion(Vector3 pos)
        {
            var newExplosion = Instantiate(explosionAnim, pos, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
        }

    }
}
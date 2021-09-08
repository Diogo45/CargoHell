using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CargoHell.Animation
{
    public class AnimationController : Singleton<AnimationController>
    {


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

        public float time { get; private set; }


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

            var planet = GameObject.Find("Planet");
            planet.GetComponent<SpriteRenderer>().sprite = _level.planetSprite;

        }

        public void SetStars(bool on)
        {
            Stars.SetActive(on);
        }


        // Update is called once per frame
        void LateUpdate()
        {
            time += Time.deltaTime;
        }

        public void Reset()
        {
            time = 0f;
        }

        public void AnimateNebula()
        {

            nebula = nebulaMat.GetVector("_Tilling");
            nebulaScrollSpeed = nebulaMat.GetVector("_ScrollSpeed");


            var nebulaFinal = new Vector4(nebula.x, 500f, nebula.z, nebula.w);
            var nebulaFinalScrollSpeed = new Vector4(30f, 0f, nebulaScrollSpeed.z, nebulaScrollSpeed.w);

            //var tillingSpeed = tillingCurve.Evaluate(time);
            //var scrollSpeed = scrollSpeedCurve.Evaluate(time);

            var tillingSpeed = tillingCurve.Evaluate(time / 500f);
            //Debug.Log(tillingSpeed);
            var scrollSpeed = scrollSpeedCurve.Evaluate(time / 15f);

            nebulaMat.SetVector("_Tilling", Vector4.Lerp(nebula, nebulaFinal, tillingSpeed));
            nebulaMat.SetVector("_ScrollSpeed", Vector4.Lerp(nebulaScrollSpeed, nebulaFinalScrollSpeed, scrollSpeed));


        }

        public void Explosion(Vector3 pos)
        {
            var newExplosion = Instantiate(explosionAnim, pos, Quaternion.identity);
            newExplosion.transform.localScale = newExplosion.transform.localScale * 5;
        }

    }
}
#pragma strict
var stayTime = 3.5;
//using UnityEngine;
//using UnityEngine.ParticleSystem;

function Update () {
  //Debug.Log(ParticleSystem.Particle.isPlaying);
  Destroy(gameObject,stayTime);
}
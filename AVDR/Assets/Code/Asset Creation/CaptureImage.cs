using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class CaptureImage : MonoBehaviour
{
    #if UNITY_EDITOR
    #region variables
    // public RenderTexture renderTexture;
    public int size = 1024;
    new public Camera camera;

    private System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    /* manager references */
    private CharacterManager characterManager;
    private AssetManager assetManager;
    private DiceManager diceManager;

    private Vector3[] originalDicePositons;

    #endregion
    #region setup

    void Awake() {
        stopwatch.Start();
        Debug.LogWarning("Capture Image Awake");
        StartCoroutine(CaptureAllCoroutine());
    }

    public IEnumerator CaptureAllCoroutine() {
        Debug.LogWarning("Capture All Assets Started.");
        yield return new WaitForEndOfFrame();
        /* get references */
        characterManager = FindFirstObjectByType<CharacterManager>(FindObjectsInactive.Include);
        assetManager = FindFirstObjectByType<AssetManager>(FindObjectsInactive.Include);
        diceManager = FindFirstObjectByType<DiceManager>(FindObjectsInactive.Include);

        

        /* check for dice */
        diceManager.GetExistingDice();
        if(diceManager.DiceInstances.Length < 7) {
            EditorApplication.Beep();
            Debug.LogError("Not enough preview dice. diceManager.DiceInstances.Length: " + diceManager.DiceInstances.Length);
            yield break;
        }

        /* store original dice positions */
        originalDicePositons = new Vector3[diceManager.DiceInstances.Length];
        for(int d = 0; d < diceManager.DiceInstances.Length; d++) {
            originalDicePositons[d] = diceManager.DiceInstances[d].transform.position;
        }

        /* gain control of the scene by turning off CharacterManager */
        characterManager.enabled = false;
        characterManager.gameObject.SetActive(false);
        
        /* capture each asset */
        yield return CaptureDiceSets(assetManager.All.diceSets);
        yield return CaptureTrays(assetManager.All.trays);
        yield return CaptureLights(assetManager.All.lightings);
        yield return CaptureEffects(assetManager.All.effects);
        
        /* finish */
        AssetDatabase.Refresh();
        Output();
        Destroy(gameObject);
        EditorApplication.isPlaying = false;
    }

    void Output() {
        stopwatch.Stop();
        string elapsed = stopwatch.Elapsed.ToString().Split('.', StringSplitOptions.None)[0];
        stopwatch.Reset();
        Debug.LogWarning("Capture All Assets completed in " + elapsed + " hh:mm:ss");
    }

    #endregion
    #region dice

    /// <summary>
    /// Iterates over the given assets and creates a screenshot and saves it.
    /// </summary>
    /// <param name="dicePairs"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator CaptureDiceSets(AKVPDice[] dicePairs) {
        yield return new WaitForEndOfFrame();
        
        /* set defaults */
        ResetScene();
        
        /* wait for everything to update */
        yield return new WaitForEndOfFrame();

        /* iterate */
        for(int i = 0; i < dicePairs.Length; i++) {
            AKVPDice pair = dicePairs[i];
            Debug.Log("Automatically Select Dice Set: " + pair.key);
            string path = "Resources/Screenshots/Dice/" + pair.key + ".png";
            
            /* replace the dice set */
            characterManager.SetDiceSet(i);

            /* wait */
            yield return new WaitForEndOfFrame();

            /* capture */
            ImageHandling.PerformSerialCapture(camera, size, path);
        }
    }

    #endregion
    #region trays

    private IEnumerator CaptureTrays(AKVPTray[] trays) {
        yield return new WaitForEndOfFrame();
        
        /* set defaults */
        // diceManager.GetExistingDice();
        ResetScene();

        /* hide dice */
        foreach(SingleDie die in diceManager.DiceInstances) {
            die.gameObject.SetActive(false);
        }
        
        /* wait for everything to update */
        yield return new WaitForEndOfFrame();

        /* iterate */
        for(int i = 0; i < trays.Length; i++) {
            AKVPTray pair = trays[i];
            Debug.Log("Automatically Select tray: " + pair.key);
            string path = "Resources/Screenshots/Trays/" + pair.key + ".png";
            
            /* replace the dice set */
            characterManager.SetTray(i);

            /* wait */
            yield return new WaitForEndOfFrame();

            /* capture */
            ImageHandling.PerformSerialCapture(camera, size, path);
        }

        /* unhide dice */
        foreach(SingleDie die in diceManager.DiceInstances) {
            die.gameObject.SetActive(true);
        }
    }

    #endregion
    #region lights

    private IEnumerator CaptureLights(AKVPLight[] lights) {
        yield return new WaitForEndOfFrame();
        
        /* set defaults */
        ResetScene();

        /* show dice */
        foreach(SingleDie die in diceManager.DiceInstances) {
            die.gameObject.SetActive(true);
        }
        
        /* wait for everything to update */
        yield return new WaitForEndOfFrame();

        /* iterate */
        for(int i = 0; i < lights.Length; i++) {
            AKVPLight pair = lights[i];
            Debug.Log("Automatically Select light: " + pair.key);
            string path = "Resources/Screenshots/Lights/" + pair.key + ".png";
            
            /* replace the dice set */
            characterManager.SetLighting(i);

            /* wait */
            yield return new WaitForSeconds(.1f);

            /* capture */
            ImageHandling.PerformSerialCapture(camera, size, path);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion
    #region effects

    private IEnumerator CaptureEffects(AKVPEffect[] effects) {
        yield return new WaitForEndOfFrame();
        
        /* set defaults */
        ResetScene();

        /* show dice */
        foreach(SingleDie die in diceManager.DiceInstances) {
            die.gameObject.SetActive(true);
        }
        
        /* wait for everything to update */
        yield return new WaitForEndOfFrame();

        /* iterate */
        for(int i = 0; i < effects.Length; i++) {
            /* remove the old set and let it clear out */
            characterManager.SetEffects(0);
            yield return new WaitForSeconds(.1f);

            /* select the new effect */
            AKVPEffect pair = effects[i];
            Debug.Log("Automatically Select effect: " + pair.key);
            string path = "Resources/Screenshots/Effects/" + pair.key + ".png";

            /* organize dice sets */
            int slots = 0;
            int offset = UnityEngine.Random.Range(0, slots);
            slots += pair.onHitPrefab != null ? 1 :0;
            slots += pair.onFinishPrefab != null ? 1 :0;
            slots += pair.trailPrefab != null ? 1 :0;
            SingleDie[] trailDice = new SingleDie[0];
            SingleDie[] hitDice = new SingleDie[0];
            SingleDie[] finishDice = new SingleDie[0];
            if(pair.onHitPrefab != null) {
                hitDice = ExtensionMethods.GetPeriodicSubsetOfArray(diceManager.DiceInstances, slots, offset + 0);
            }
            if(pair.onFinishPrefab != null) {
                finishDice = ExtensionMethods.GetPeriodicSubsetOfArray(diceManager.DiceInstances, slots, offset + 1);
            }
            if(pair.trailPrefab != null) {
                trailDice = ExtensionMethods.GetPeriodicSubsetOfArray(diceManager.DiceInstances, slots, offset + 2);
            }

            /* record the finish positions of the trail dice */
            ResetDicePositions();
            Vector3[] finalPositions = new Vector3[trailDice.Length];
            Vector3[] startPositions = new Vector3[trailDice.Length];
            for(int t = 0; t < trailDice.Length; t++) {
                Rigidbody rigidbody = trailDice[t].GetComponent<Rigidbody>();
                rigidbody.isKinematic = true;
                startPositions[t] = trailDice[t].transform.position * 2;
                finalPositions[t] = trailDice[t].transform.position;
                
                /* move the trail dice to their starting positions */
                trailDice[t].transform.position = startPositions[t];
            }
            
            /* replace the effects */
            characterManager.SetEffects(i);

            /* set up the environment effect if it exists */
            if(pair.environmentPrefab != null) {
                GameObject parent = FindFirstObjectByType<ParticleEffectManager>().gameObject;
                ParticleSystem[] children = parent.GetComponentsInChildren<ParticleSystem>();
                ParticleSystem envSystem = null;
                foreach(ParticleSystem child in children) {
                    if(child.gameObject.name == pair.environmentPrefab.name + "(Clone)") {
                        envSystem = child;
                    }
                }
                if(envSystem != null) {
                    ParticleSystem.MainModule main = envSystem.main;
                    main.prewarm = true;
                    ParticleSystem.EmissionModule emission = envSystem.emission;
                    emission.rateOverTimeMultiplier *= pair.captureMultiplier;
                    main.maxParticles = (int)(main.maxParticles * pair.captureMultiplier);
                    envSystem.Play();
                    yield return new WaitForEndOfFrame();
                }
            }

            /* validate peak times */
            foreach(AKVPEffect effect in assetManager.All.effects) {
                if(effect.CheckValidity() == false) {
                    Debug.LogError("Breaking the CaptureImage loop because of an invalid AKVPEffect. See above.");
                    Debug.LogError("EFFECTS NOT RENDERED!");
                    yield break;
                }
            }

            /* get ready for our timing loop */
            List<GameObject> deleteList = new List<GameObject>();
            float renderDuration = pair.RenderDuration;
            float startTime = Time.time;
            float finishTime = startTime + renderDuration;
            float hitSpawnTime = finishTime - pair.hitPeakTime;
            float finishSpawnTime = finishTime - pair.finishPeakTime;
            bool didSpawnHit = false;
            bool didSpawnFinish = false;

            /* timing loop */
            while(Time.time <= finishTime) {
                /* move trail dice around so the trail appears */
                for(int m = 0; m < trailDice.Length; m++) {
                    SingleDie die = trailDice[m];
                    float percentage = TimePercentage(startTime, finishTime, renderDuration);
                    die.transform.position = Vector3.Lerp(startPositions[m], finalPositions[m], percentage);
                }
                /* instantiate particle effects manually */
                if(
                    pair.onHitPrefab != null && 
                    didSpawnHit == false &&
                    Time.time >= hitSpawnTime
                ) {
                    didSpawnHit = true;
                    foreach(SingleDie d in hitDice) {
                        GameObject instance = Instantiate(pair.onHitPrefab, d.transform.position, Quaternion.identity);
                        deleteList.Add(instance);
                    }
                }
                if(
                    pair.onFinishPrefab != null &&
                    didSpawnFinish == false &&
                    Time.time >= finishSpawnTime
                ) {
                    didSpawnFinish = true;
                    foreach(SingleDie d in finishDice) {
                        GameObject instance = Instantiate(pair.onFinishPrefab, d.transform.position, Quaternion.identity);
                        deleteList.Add(instance);
                    }
                }
                yield return new WaitForEndOfFrame();
            }
            /* totally reset the dice to ensure uniformity across thumbnails */
            ResetDicePositions();

            /* capture */
            yield return new WaitForEndOfFrame();
            ImageHandling.PerformSerialCapture(camera, size, path);

            /* clear particle effects */
            for(int e = 0; e < deleteList.Count; e++) {
                GameObject gameObject = deleteList[e];
                gameObject.SetActive(false);
                Destroy(gameObject);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void ResetScene() {
        characterManager.SetDiceSet(0);
        characterManager.SetTray(0);
        characterManager.SetLighting(0);
        characterManager.SetEffects(0);
        ResetDicePositions();
    }
    private void ResetDicePositions() {
        for(int d = 0; d < diceManager.DiceInstances.Length; d++) {
            diceManager.DiceInstances[d].transform.position = originalDicePositons[d];
        }
    }

    private float TimePercentage(float start, float finish, float duration) {
        float progress = finish - start;
        if(duration <= 0) {
            Debug.LogError("duration cannot be zero.");
            return 0;
        }
        return progress / duration;
    }

    #endregion
    #endif
}

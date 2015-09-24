using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEditor;

/*
 * DBXV Character Creation
 *
 * The MIT License (MIT)
 *
 * Copyright (c) 2015 Ruben Garcia <rubengcdev@gmail.com> <rubengc.com>
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 *
 * Dragon Ball Xenoverse and all their content used on this project are property of BIRD STUDIO / SHUEISHA, TOEI ANIMATION.
 */

public class MenuAction : MonoBehaviour {

    public Global global;
    public Character character;
    public int currentIndex = 0;
    public string characterColorName;
    public Image currentImage;

    // Head parts switchers

    public void NextHeadPart(string part) {
        GameObject[] parts;

        if(part == "Face_base")
            parts = global.face.bases;
        else if(part == "Face_ear")
            parts = global.face.ears;
        else if(part == "Face_eye")
            parts = global.face.eyes;
        else if(part == "Face_forehead")
            parts = global.face.foreheads;
        else if(part == "Face_nose")
            parts = global.face.noses;
        else
            parts = global.face.hairs;

        currentIndex++;

        if(currentIndex > parts.Length - 1)
            currentIndex = 0;

        character.SetPart(parts[currentIndex], part);
        transform.Find("Label").GetComponent<Text>().text = "Type " + (currentIndex + 1);
    }

    public void PrevHeadPart(string part) {
        GameObject[] parts;

        if(part == "Face_base")
            parts = global.face.bases;
        else if(part == "Face_ear")
            parts = global.face.ears;
        else if(part == "Face_eye")
            parts = global.face.eyes;
        else if(part == "Face_forehead")
            parts = global.face.foreheads;
        else if(part == "Face_nose")
            parts = global.face.noses;
        else
            parts = global.face.hairs;

        currentIndex--;

        if(currentIndex < 0)
            currentIndex = parts.Length-1;

        character.SetPart(parts[currentIndex], part);
        transform.Find("Label").GetComponent<Text>().text = "Type " + (currentIndex + 1);
    }

    // Outfit parts switchers

    public void NextOutfitPart(string part) {
        currentIndex++;

        if(currentIndex > global.outfits.Length - 1)
            currentIndex = 0;

        character.SetPart(global.outfits[currentIndex], part);
        transform.Find("Label").GetComponent<Text>().text = "Outfit " + (currentIndex + 1);
    }

    public void PrevOutfitPart(string part) {
        currentIndex--;

        if(currentIndex < 0)
            currentIndex = global.outfits.Length - 1;

        character.SetPart(global.outfits[currentIndex], part);
        transform.Find("Label").GetComponent<Text>().text = "Outfit " + (currentIndex + 1);
    }

    // When the users wants change a color, we store the current part name (ChangeCharacterColorName) and UI Image clicked (ChangeImageColor)
    // After we open the color picker to assign the color (ChangeCharacterColor)
    public void ChangeCharacterColorName(string name) {
        characterColorName = name;
    }
    
    public void ChangeImageColor(Image image) {
        currentImage = image;
    }

    public void ChangeCharacterColor(string hexColor) {
        if(hexColor.StartsWith("#"))
            hexColor.Replace("#", "");

        Color color = HexToColor(hexColor);

        // Send the new color to the character
        character.SetColor(color, characterColorName);
        // Updates the UI image with the picked color
        currentImage.color = color;
    }

    public void SaveCharacterAsPrefab() {
        string name = transform.GetComponentInChildren<InputField>().text.Replace(" ", "_");

        // Clone character GameObject
        GameObject characterClone = Instantiate(character.gameObject);

        // Character script initializes the character parts, so we want to maintain the current configuration disabling the sript
        characterClone.GetComponent<Character>().enabled = false;
        characterClone.GetComponent<RotateOnDrag>().enabled = false;

        // Creates character storage folders if don't exists
        if(!AssetDatabase.IsValidFolder("Assets/SavedCharacters"))
            AssetDatabase.CreateFolder("Assets", "SavedCharacters");
        if(!AssetDatabase.IsValidFolder("Assets/SavedCharacters/" + name))
            AssetDatabase.CreateFolder("Assets/SavedCharacters", name);
        if(!AssetDatabase.IsValidFolder("Assets/SavedCharacters/" + name +  "/Materials"))
            AssetDatabase.CreateFolder("Assets/SavedCharacters/" + name, "Materials");

        // Clone all character materials
        foreach(Transform child in characterClone.GetComponentsInChildren<Transform>()) {
            if(child.GetComponent<Renderer>()) {
                foreach(Material material in child.GetComponent<Renderer>().materials) {
                    AssetDatabase.CreateAsset(material, "Assets/SavedCharacters/" + name + "/Materials/" + material.name.Replace(" (Instance)", "") + ".mat");
                }
            }
        }

        // Creates and store the character prefab based on character clone GameObject
        Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/SavedCharacters/" + name + "/" + name + ".prefab");
        PrefabUtility.ReplacePrefab(characterClone, prefab, ReplacePrefabOptions.ConnectToPrefab);

        // Finally destroy the character clone
        Destroy(characterClone);
    }

    private Color HexToColor(string hex) {
        float a = 0;

        float r = (HexToFloat(hex.Substring(1, 1)) + HexToFloat(hex.Substring(0, 1)) * 16) / 255;
        float g = (HexToFloat(hex.Substring(3, 1)) + HexToFloat(hex.Substring(2, 1)) * 16) / 255;
        float b = (HexToFloat(hex.Substring(5, 1)) + HexToFloat(hex.Substring(4, 1)) * 16) / 255;

        if(hex.Length == 6)
            a = (HexToFloat(hex.Substring(7, 1)) + HexToFloat(hex.Substring(6, 1)) * 16) / 255;
        else
            a = 1;

        return new Color(r, g, b, a);
    }

    private float HexToFloat(string hexChar) {
        switch(hexChar) {
            case "1": return 1;
            case "2": return 2;
            case "3": return 3;
            case "4": return 4;
            case "5": return 5;
            case "6": return 6;
            case "7": return 7;
            case "8": return 8;
            case "9": return 9;
            case "A": return 10;
            case "B": return 11;
            case "C": return 12;
            case "D": return 13;
            case "E": return 14;
            case "F": return 15;
            default: return 0;
        }
    }
}

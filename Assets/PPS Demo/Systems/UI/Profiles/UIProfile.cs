using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using PPS;
using UnityEngine.UI;
using Object = UnityEngine.Object;

[Serializable]
public class UIProfile : Profile {

    [Serializable]
    public struct ScoreContainer {
        public Text label;
        public Text value;
    }

    [Serializable]
    public struct HealthContainer {
        public Image bar;
        public Image health;
    }

    [SerializeField]
    private ScoreContainer localScore;
    [SerializeField]
    private ScoreContainer enemyScore;
    [SerializeField]
    private HealthContainer healthContainer;

    public UIProfile(GameObject gameObject) : base(gameObject) {
        UIReferencing referencing = gameObject.GetComponent<UIReferencing>();

        this.localScore = referencing.localScore;
        this.localScore.label.text = "YOUR SCORE";
        this.enemyScore = referencing.enemyScore;
        this.healthContainer = referencing.healthContainer;

        Object.DestroyImmediate(referencing);
    }

    public void AddLocalScore(int score) {
        this.localScore.value.text = (int.Parse(this.localScore.value.text) + score).ToString();
    }

    public void AddEnemyScore(int score) {
        this.enemyScore.value.text = (int.Parse(this.enemyScore.value.text) + score).ToString();
    }

    public void SetEnemyScoreLabel(string text) {
        this.enemyScore.label.text = text;
    }

    public void SetHealth(float percentage) {
        float parentWidth = this.healthContainer.bar.rectTransform.rect.width;
        this.healthContainer.health.rectTransform.sizeDelta = new Vector2(parentWidth * percentage, 0);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuideButton
{
    /// <summary>
    /// Implement listner to TipAndGuidesScreen.OnTriggerMenuHighlight in wake
    /// </summary>
    /// <param name="highlight"></param>
    void OnHighlightCall(MenuHighlight highlight);
}

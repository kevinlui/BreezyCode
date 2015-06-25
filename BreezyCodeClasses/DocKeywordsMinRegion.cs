using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BreezyCode.Classes
{
    ///
    /// - Given a document and a set of keywords, assuming all keywords appear in the document at least once. 
    /// - Find the region (start and end position) that is minimal in length that covers all the keywords at least once.
    /// 
    public class DocKeywordsMinRegion
    {
        private readonly Dictionary<string, List<int>> _docIdx = new Dictionary<string, List<int>>();

        /// <summary>
        /// Constructor taking in a document string, turning it into an Inverted Index _docIdx
        /// </summary>
        /// <param name="docContent"></param>
        public DocKeywordsMinRegion(string docContent)
        {
            if (string.IsNullOrEmpty(docContent))
                return;

            string[] tokens = Regex.Split(docContent, @"\W+");
            for (int i = 0; i < tokens.Length; i++)
            {
                string t = tokens[i];
                if (!_docIdx.ContainsKey(t))
                    _docIdx.Add(t, new List<int>());

                _docIdx[t].Add(i);
            }
        }

        /// <summary>
        /// FindMinRegion()
        /// 
        /// E.g. Sample document string as follows: 
        ///     "w1 w4 w1 w3 w4 w5 w3 w1 w4 w5 w3 w3 w1 w2 w4 w2 w3 w5 w4"
        ///   
        ///     would _result in a _docIdx as follows:
        ///      { "w1", { 0, 2, 7, 12 } },
        ///      { "w2", { 13, 15 } },
        ///      { "w3", { 3, 6, 10, 11, 16 } },
        ///      { "w4", { 1, 4, 8, 14, 18 } },
        ///      { "w5", { 5, 9, 17 } }
        ///
        /// Search Keywords: { "w2", "w4", "w5" } would _result in 
        /// Answer: { size=3, pos = {14, 17}} ( i.e. { {"w2", 15}, {"w4", 14}, {"w5", 17} } )
        /// 
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns>A Tyuple representing the regionsSize, and the Start/End positions</returns>
        public Tuple<int, Tuple<int, int>> FindMinRegion(List<string> keywords)
        {
            if (keywords == null || keywords.Count == 0 || _docIdx.Count == 0)
                return null;

            // Extract corresponding inverted index for the keywords
            var dictActive = _docIdx.Where(pair => keywords.Contains(pair.Key));
            // Assume all keywords must be found in doc, otherwise return null for invalid input case
            if (dictActive.Count() != keywords.Count) 
                return null;

            // Initialize data structures and start recursion:
            // 1) lstActiveIdx: relevant entries from _docIdx according to keywords
            // 2) keywordsIdx: position index to indicate which combination we looking
            // 3) result: the Tuple contain the structure {resgionSize, {startPos, endPos}}
            var lstActiveIdx = dictActive.Select(p => p.Value).ToList();
            var keywordsIdx = (new int[keywords.Count]).Select(x => 0).ToList();
            var result = new Tuple<int, Tuple<int, int>>(Int32.MaxValue, new Tuple<int, int>(0, 0));

            recurse(lstActiveIdx, keywordsIdx, ref result);

            return result;
        }

        private void recurse(List<List<int>> lstActiveIdx, List<int> keywordsIdx, ref Tuple<int, Tuple<int, int>> result)
        {
            // Calc region size, update _result if new smaller
            regionSize(lstActiveIdx, keywordsIdx, ref result);

            // Move keywordsIdx forward, two conditions to keep incrementing
            // Condition 1: moving index has reached max in current word index list
            //  - Action: reset dimension index to 0 and move to next lower dimension
            // Condition 2: value on moving index is already bigger than previous dimenion
            //  - Action: fast track moving index to end
            int d = keywordsIdx.Count - 1;
            bool condition1 = false, condition2 = false;
            while ((condition1 = (d >= 0 && ++keywordsIdx[d] >= lstActiveIdx[d].Count)) ||
                (condition2 = ((d > 0 && keywordsIdx[d] > 0) && (lstActiveIdx[d][keywordsIdx[d] - 1] > lstActiveIdx[d - 1][keywordsIdx[d - 1]]))))
            {
                if (condition1)
                    keywordsIdx[d--] = 0;   // Action1: reset index to 0 and move to next lower dimenion
                else if (condition2)
                    keywordsIdx[d] = lstActiveIdx[d].Count-1;   // Action2: fast forward on current dimension
            }

            // Reached terminating case if d dropped below zero
            if (d < 0) return;

            // With keywordsIdx moved to next, recurse
            recurse(lstActiveIdx, keywordsIdx, ref result);
        }

        private void regionSize(List<List<int>> lstActiveIdx, List<int> keywordsIdx, ref Tuple<int, Tuple<int, int>> result)
        {
            // Calc region size for words referred to by keywordsIdx
            List<int> lstWordPos = new List<int>(keywordsIdx.Count);
            for (int wordPos = 0; wordPos < keywordsIdx.Count; wordPos++)
                lstWordPos.Add(lstActiveIdx[wordPos].ElementAt(keywordsIdx[wordPos]));

            // If keywordsIdx constitues a smaller region than previos _result, overwrite _result
            if (lstWordPos.Max() - lstWordPos.Min() < result.Item1)
            {
                result = new Tuple<int, Tuple<int, int>>(lstWordPos.Max() - lstWordPos.Min(),
                    new Tuple<int, int>(lstWordPos.Min(), lstWordPos.Max()));
            }
        }
    }

    /*
    public Tuple<int, Tuple<int, int>> FindMinRegion(List<string> keywords)
    {
        if (keywords == null || keywords.Count == 0 || _docInvertedIndex.Count == 0)
            return null;

        // clear any previous result first
        _result = new Tuple<int, Tuple<int, int>>(Int32.MaxValue, new Tuple<int, int>(0, 0));

        // Extract the corresponding indices for the keywords first
        _lstActiveInvertedIndex = new List<List<int>>(
            _docInvertedIndex.Where(pair => keywords.Contains(pair.Key)).Select(p => p.Value));
        if (_lstActiveInvertedIndex.Count() != keywords.Count) return null;

        // initialize a currentIndex which refers to a keywords of the word pos 
        // from main _docInvertedIndex
        List<int> currentIndex = new List<int>(keywords.Count);
        for (int i = 0; i < keywords.Count; i++)
            currentIndex.Add(0);

        // recursion to generate Combinations, working from the last dimension
        recurse(currentIndex, keywords.Count-1);

        return _result;
    }

    private void recurse(List<int> keywords, int curDim)
    {
        // Update _result if smaller region found
        Tuple<int, int> region = getRegionSize(keywords);
        if (region.Item2 - region.Item1 < _result.Item1)
            _result = new Tuple<int, Tuple<int, int>>(region.Item2 - region.Item1, new Tuple<int, int>(region.Item1, region.Item2));

        // Move to next Combo, nextKeywordsIdx() will return false if termination case
        if (nextKeywordsIdx(ref keywords, ref curDim))
            recurse(keywords, curDim);
    }

    /// <summary>
    /// Move on to next Combination we want to visit,  is given by curDim
    /// </summary>
    /// <param name="keywordsIndex">keywords' inverted Index</param>
    /// <param name="keywords">index indicating the current keywords</param>
    /// <param name="curDim">current dimension being iterated</param>
    /// <returns>true if continue on, false if we exhausted all Combos</returns>
    private bool nextKeywordsIdx(ref List<int> keywords, ref int curDim)
    {
        // Optimization: 
        //  No need to visit remaining bigger values in current dimension, if the current value
        //  is already bigger than the lower dimenion current value of the keywords
        if (curDim > 0 &&
            (_lstActiveInvertedIndex[curDim].ElementAt(keywords[curDim]) >
            _lstActiveInvertedIndex[curDim - 1].ElementAt(keywords[curDim - 1])))
        {
            // fast track it to the end
            keywords[curDim] = _lstActiveInvertedIndex[curDim].Count - 1;
        }

        bool dimChanged = false;

        // ++keywords[curDim] increment on current dimension
        // But while loop for checking if reach max on current dimension, then need to increment on lower dimension
        while (++keywords[curDim] >= _lstActiveInvertedIndex[curDim].Count)
        {
            // Track we had changed dimension, which we'll use right after the while-loop
            dimChanged = true;

            // adjust the curDim index, shift down a dimension if needed, and detect terminating case
            keywords[curDim] = 0;
                
            // Shift down a dimension,
            // Check if reached beyond lowest dimension, in which case we are done
            if (--curDim < 0)
                return false;

            // curDim had been decremented to previous dimension, we would keep 
            // executing the while loop condition which will perfomr the ++keywords[curDim]

        }

        // reset curDim to last dimension if dim had changed above
        if (dimChanged)
            curDim = keywords.Count - 1;

        // keep going by default
        return true;
    }
     */
}

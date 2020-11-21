using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace mini_java_compiler
{


		[Language("Java", "1.6", "An (almost) complete java parser")]
		public partial class JavaGrammar : Grammar
		{
			private readonly TerminalSet mSkipTokensInPreview = new TerminalSet(); //used in token preview for conflict resolution

			public JavaGrammar()
			{
				GrammarComments = "NOTE: This grammar does not parse hex floating point literals.";

				var singleLineComment = new CommentTerminal("SingleLineComment", "//", "\r", "\n", "\u2085", "\u2028", "\u2029");
				var delimitedComment = new CommentTerminal("DelimitedComment", "/*", "*/");
				NonGrammarTerminals.Add(singleLineComment);
				NonGrammarTerminals.Add(delimitedComment);

				MarkPunctuation(";", ",", "(", ")", "{", "}", "[", "]", ":", "@");

				InitializeSyntax();
			}

			public override void OnScannerSelectTerminal(ParsingContext context)
			{
				if (context.Source.PreviewChar == '>' && context.Status == ParserStatus.Previewing)
				{
					context.CurrentTerminals.Clear();
					context.CurrentTerminals.Add(ToTerm(">", "gt")); //select the ">" terminal
				}
				base.OnScannerSelectTerminal(context);
			}

			private void ResolveConflicts(ParsingContext context, CustomParserAction action)
			{
			}
			/* BROKEN
			public override void OnResolvingConflict(ConflictResolutionArgs args)
			{
			  switch (args.Context.CurrentParserInput.Term.Name)
			  {
				case "[":
				  {
					args.Scanner.BeginPreview();
					var preview = args.Scanner.GetToken();
					string previewSym = preview.Terminal.Name;
					args.Result = previewSym == "]" ? PreferredActionType.Reduce : PreferredActionType.Shift;
					args.Scanner.EndPreview(true);
					return;
				  }
				case "dot":
				  {
					args.Scanner.BeginPreview();
					var preview = args.Scanner.GetToken();
					string previewSym = preview.Text;
					if (previewSym == "<")
					{
					  // skip over any type arguments
					  int depth = 0;
					  do
					  {
						if (previewSym == "<")
						{
						  ++depth;
						}
						else if (previewSym == ">")
						{
						  --depth;
						}
						preview = args.Scanner.GetToken();
						previewSym = preview.Text;
					  } while (depth > 0 && preview.Terminal != Eof);
					}
					switch (previewSym)
					{
					  case "new":
					  case "super":
					  case "this":
						args.Result = PreferredActionType.Reduce;
						break;
					  default:
						args.Result = PreferredActionType.Shift;
						break;
					}
					args.Scanner.EndPreview(true);
					return;
				  }
				case "lt":
				  {
					args.Scanner.BeginPreview();
					int ltCount = 0;
					string previewSym;
					while (true)
					{
					  //Find first token ahead (using preview mode) that is either end of generic parameter (">") or something else
					  Token preview;
					  do
					  {
						preview = args.Scanner.GetToken();
					  } while (mSkipTokensInPreview.Contains(preview.Terminal) && preview.Terminal != Eof);
					  //See what did we find
					  previewSym = preview.Terminal.Name;
					  if ((previewSym == "<") || (previewSym == "lt"))
					  {
						ltCount++;
					  }
					  else if (((previewSym == ">") || (previewSym == "gt")) && ltCount > 0)
					  {
						ltCount--;
						continue;
					  }
					  else
						break;
					}
					//if we see ">", then it is type argument, not operator
					if ((previewSym == ">") || (previewSym == "gt"))
					{
					  args.Result = PreferredActionType.Shift;
					}
					else
					{
					  args.Result = PreferredActionType.Reduce;
					}
					args.Scanner.EndPreview(true);
					//keep previewed tokens; important to keep ">>" matched to two ">" symbols, not one combined symbol (see method below)
					return;
				  }
			  }
			} */
		}
		partial class JavaGrammar
		{
			public static NumberLiteral CreateJavaNumber(string name)
			{
				var term = new NumberLiteral(name, NumberOptions.AllowStartEndDot)
				{
					DefaultIntTypes = new[] { TypeCode.Int32 },
					DefaultFloatType = TypeCode.Double
				};
				term.AddPrefix("0x", NumberOptions.Hex);
				term.AddSuffix("l", TypeCode.Int64);
				term.AddSuffix("f", TypeCode.Single);
				term.AddSuffix("d", TypeCode.Double);
				return term;
			}

			public static StringLiteral CreateJavaString(string name)
			{
				return new StringLiteral(name, "\"", StringOptions.AllowsAllEscapes);
			}

			public static StringLiteral CreateJavaChar(string name)
			{
				return new StringLiteral(name, "'", StringOptions.IsChar | StringOptions.AllowsAllEscapes);
			}

			public static Terminal CreateJavaNull(string name)
			{
				return new KeyTerm("null", name);
			}

			private static void InitializeCharacterSet(ref string characterSet, IEnumerable<int[]> ranges)
			{
				var sbCharSet = new StringBuilder();
				foreach (var range in ranges)
				{
					for (int i = range[0]; i <= range[1]; ++i)
					{
						sbCharSet.Append((Char)i);
					}
				}
				characterSet = sbCharSet.ToString();
			}

			private static string _validIdentifierStartCharacters;
			public static string ValidIdentifierStartCharacters
			{
				get
				{
					if (_validIdentifierStartCharacters == null)
					{
						InitializeCharacterSet(ref _validIdentifierStartCharacters, ValidIdentifierStartCharactersRanges);
					}
					return _validIdentifierStartCharacters;
				}
			}

			private static string _validIdentifierCharacters;
			public static string ValidIdentifierCharacters
			{
				get
				{
					if (_validIdentifierCharacters == null)
					{
						InitializeCharacterSet(ref _validIdentifierCharacters, ValidIdentifierCharactersRanges);
					}
					return _validIdentifierCharacters;
				}
			}

			private static readonly int[][] ValidIdentifierStartCharactersRanges = new[]
																			{
#region Identifier Start Character Ranges
		                                                               	
		                                                               		new[] {36, 36},
																			   new[] {65, 90},
																			   new[] {95, 95},
																			   new[] {97, 122},
																			   new[] {162, 165},
																			   new[] {170, 170},
																			   new[] {181, 181},
																			   new[] {186, 186},
																			   new[] {192, 214},
																			   new[] {216, 246},
																			   new[] {248, 566},
																			   new[] {592, 705},
																			   new[] {710, 721},
																			   new[] {736, 740},
																			   new[] {750, 750},
																			   new[] {890, 890},
																			   new[] {902, 902},
																			   new[] {904, 906},
																			   new[] {908, 908},
																			   new[] {910, 929},
																			   new[] {931, 974},
																			   new[] {976, 1013},
																			   new[] {1015, 1019},
																			   new[] {1024, 1153},
																			   new[] {1162, 1230},
																			   new[] {1232, 1269},
																			   new[] {1272, 1273},
																			   new[] {1280, 1295},
																			   new[] {1329, 1366},
																			   new[] {1369, 1369},
																			   new[] {1377, 1415},
																			   new[] {1488, 1514},
																			   new[] {1520, 1522},
																			   new[] {1569, 1594},
																			   new[] {1600, 1610},
																			   new[] {1646, 1647},
																			   new[] {1649, 1747},
																			   new[] {1749, 1749},
																			   new[] {1765, 1766},
																			   new[] {1774, 1775},
																			   new[] {1786, 1788},
																			   new[] {1791, 1791},
																			   new[] {1808, 1808},
																			   new[] {1810, 1839},
																			   new[] {1869, 1871},
																			   new[] {1920, 1957},
																			   new[] {1969, 1969},
																			   new[] {2308, 2361},
																			   new[] {2365, 2365},
																			   new[] {2384, 2384},
																			   new[] {2392, 2401},
																			   new[] {2437, 2444},
																			   new[] {2447, 2448},
																			   new[] {2451, 2472},
																			   new[] {2474, 2480},
																			   new[] {2482, 2482},
																			   new[] {2486, 2489},
																			   new[] {2493, 2493},
																			   new[] {2524, 2525},
																			   new[] {2527, 2529},
																			   new[] {2544, 2547},
																			   new[] {2565, 2570},
																			   new[] {2575, 2576},
																			   new[] {2579, 2600},
																			   new[] {2602, 2608},
																			   new[] {2610, 2611},
																			   new[] {2613, 2614},
																			   new[] {2616, 2617},
																			   new[] {2649, 2652},
																			   new[] {2654, 2654},
																			   new[] {2674, 2676},
																			   new[] {2693, 2701},
																			   new[] {2703, 2705},
																			   new[] {2707, 2728},
																			   new[] {2730, 2736},
																			   new[] {2738, 2739},
																			   new[] {2741, 2745},
																			   new[] {2749, 2749},
																			   new[] {2768, 2768},
																			   new[] {2784, 2785},
																			   new[] {2801, 2801},
																			   new[] {2821, 2828},
																			   new[] {2831, 2832},
																			   new[] {2835, 2856},
																			   new[] {2858, 2864},
																			   new[] {2866, 2867},
																			   new[] {2869, 2873},
																			   new[] {2877, 2877},
																			   new[] {2908, 2909},
																			   new[] {2911, 2913},
																			   new[] {2929, 2929},
																			   new[] {2947, 2947},
																			   new[] {2949, 2954},
																			   new[] {2958, 2960},
																			   new[] {2962, 2965},
																			   new[] {2969, 2970},
																			   new[] {2972, 2972},
																			   new[] {2974, 2975},
																			   new[] {2979, 2980},
																			   new[] {2984, 2986},
																			   new[] {2990, 2997},
																			   new[] {2999, 3001},
																			   new[] {3065, 3065},
																			   new[] {3077, 3084},
																			   new[] {3086, 3088},
																			   new[] {3090, 3112},
																			   new[] {3114, 3123},
																			   new[] {3125, 3129},
																			   new[] {3168, 3169},
																			   new[] {3205, 3212},
																			   new[] {3214, 3216},
																			   new[] {3218, 3240},
																			   new[] {3242, 3251},
																			   new[] {3253, 3257},
																			   new[] {3261, 3261},
																			   new[] {3294, 3294},
																			   new[] {3296, 3297},
																			   new[] {3333, 3340},
																			   new[] {3342, 3344},
																			   new[] {3346, 3368},
																			   new[] {3370, 3385},
																			   new[] {3424, 3425},
																			   new[] {3461, 3478},
																			   new[] {3482, 3505},
																			   new[] {3507, 3515},
																			   new[] {3517, 3517},
																			   new[] {3520, 3526},
																			   new[] {3585, 3632},
																			   new[] {3634, 3635},
																			   new[] {3647, 3654},
																			   new[] {3713, 3714},
																			   new[] {3716, 3716},
																			   new[] {3719, 3720},
																			   new[] {3722, 3722},
																			   new[] {3725, 3725},
																			   new[] {3732, 3735},
																			   new[] {3737, 3743},
																			   new[] {3745, 3747},
																			   new[] {3749, 3749},
																			   new[] {3751, 3751},
																			   new[] {3754, 3755},
																			   new[] {3757, 3760},
																			   new[] {3762, 3763},
																			   new[] {3773, 3773},
																			   new[] {3776, 3780},
																			   new[] {3782, 3782},
																			   new[] {3804, 3805},
																			   new[] {3840, 3840},
																			   new[] {3904, 3911},
																			   new[] {3913, 3946},
																			   new[] {3976, 3979},
																			   new[] {4096, 4129},
																			   new[] {4131, 4135},
																			   new[] {4137, 4138},
																			   new[] {4176, 4181},
																			   new[] {4256, 4293},
																			   new[] {4304, 4344},
																			   new[] {4352, 4441},
																			   new[] {4447, 4514},
																			   new[] {4520, 4601},
																			   new[] {4608, 4614},
																			   new[] {4616, 4678},
																			   new[] {4680, 4680},
																			   new[] {4682, 4685},
																			   new[] {4688, 4694},
																			   new[] {4696, 4696},
																			   new[] {4698, 4701},
																			   new[] {4704, 4742},
																			   new[] {4744, 4744},
																			   new[] {4746, 4749},
																			   new[] {4752, 4782},
																			   new[] {4784, 4784},
																			   new[] {4786, 4789},
																			   new[] {4792, 4798},
																			   new[] {4800, 4800},
																			   new[] {4802, 4805},
																			   new[] {4808, 4814},
																			   new[] {4816, 4822},
																			   new[] {4824, 4846},
																			   new[] {4848, 4878},
																			   new[] {4880, 4880},
																			   new[] {4882, 4885},
																			   new[] {4888, 4894},
																			   new[] {4896, 4934},
																			   new[] {4936, 4954},
																			   new[] {5024, 5108},
																			   new[] {5121, 5740},
																			   new[] {5743, 5750},
																			   new[] {5761, 5786},
																			   new[] {5792, 5866},
																			   new[] {5870, 5872},
																			   new[] {5888, 5900},
																			   new[] {5902, 5905},
																			   new[] {5920, 5937},
																			   new[] {5952, 5969},
																			   new[] {5984, 5996},
																			   new[] {5998, 6000},
																			   new[] {6016, 6067},
																			   new[] {6103, 6103},
																			   new[] {6107, 6108},
																			   new[] {6176, 6263},
																			   new[] {6272, 6312},
																			   new[] {6400, 6428},
																			   new[] {6480, 6509},
																			   new[] {6512, 6516},
																			   new[] {7424, 7531},
																			   new[] {7680, 7835},
																			   new[] {7840, 7929},
																			   new[] {7936, 7957},
																			   new[] {7960, 7965},
																			   new[] {7968, 8005},
																			   new[] {8008, 8013},
																			   new[] {8016, 8023},
																			   new[] {8025, 8025},
																			   new[] {8027, 8027},
																			   new[] {8029, 8029},
																			   new[] {8031, 8061},
																			   new[] {8064, 8116},
																			   new[] {8118, 8124},
																			   new[] {8126, 8126},
																			   new[] {8130, 8132},
																			   new[] {8134, 8140},
																			   new[] {8144, 8147},
																			   new[] {8150, 8155},
																			   new[] {8160, 8172},
																			   new[] {8178, 8180},
																			   new[] {8182, 8188},
																			   new[] {8255, 8256},
																			   new[] {8276, 8276},
																			   new[] {8305, 8305},
																			   new[] {8319, 8319},
																			   new[] {8352, 8369},
																			   new[] {8450, 8450},
																			   new[] {8455, 8455},
																			   new[] {8458, 8467},
																			   new[] {8469, 8469},
																			   new[] {8473, 8477},
																			   new[] {8484, 8484},
																			   new[] {8486, 8486},
																			   new[] {8488, 8488},
																			   new[] {8490, 8493},
																			   new[] {8495, 8497},
																			   new[] {8499, 8505},
																			   new[] {8509, 8511},
																			   new[] {8517, 8521},
																			   new[] {8544, 8579},
																			   new[] {12293, 12295},
																			   new[] {12321, 12329},
																			   new[] {12337, 12341},
																			   new[] {12344, 12348},
																			   new[] {12353, 12438},
																			   new[] {12445, 12447},
																			   new[] {12449, 12543},
																			   new[] {12549, 12588},
																			   new[] {12593, 12686},
																			   new[] {12704, 12727},
																			   new[] {12784, 12799},
																			   new[] {13312, 19893},
																			   new[] {19968, 40869},
																			   new[] {40960, 42124},
																			   new[] {44032, 55203},
																			   new[] {63744, 64045},
																			   new[] {64048, 64106},
																			   new[] {64256, 64262},
																			   new[] {64275, 64279},
																			   new[] {64285, 64285},
																			   new[] {64287, 64296},
																			   new[] {64298, 64310},
																			   new[] {64312, 64316},
																			   new[] {64318, 64318},
																			   new[] {64320, 64321},
																			   new[] {64323, 64324},
																			   new[] {64326, 64433},
																			   new[] {64467, 64829},
																			   new[] {64848, 64911},
																			   new[] {64914, 64967},
																			   new[] {65008, 65020},
																			   new[] {65075, 65076},
																			   new[] {65101, 65103},
																			   new[] {65129, 65129},
																			   new[] {65136, 65140},
																			   new[] {65142, 65276},
																			   new[] {65284, 65284},
																			   new[] {65313, 65338},
																			   new[] {65343, 65343},
																			   new[] {65345, 65370},
																			   new[] {65381, 65470},
																			   new[] {65474, 65479},
																			   new[] {65482, 65487},
																			   new[] {65490, 65495},
																			   new[] {65498, 65500},
																			   new[] {65504, 65505},
																			   new[] {65509, 65510},
#endregion
		                                                               	};

			private static readonly int[][] ValidIdentifierCharactersRanges = new[]
																	{
#region Identifier Character Ranges
		                                                    		new[] {0, 8},
																	new[] {14, 27},
																	new[] {36, 36},
																	new[] {48, 57},
																	new[] {65, 90},
																	new[] {95, 95},
																	new[] {97, 122},
																	new[] {127, 159},
																	new[] {162, 165},
																	new[] {170, 170},
																	new[] {173, 173},
																	new[] {181, 181},
																	new[] {186, 186},
																	new[] {192, 214},
																	new[] {216, 246},
																	new[] {248, 566},
																	new[] {592, 705},
																	new[] {710, 721},
																	new[] {736, 740},
																	new[] {750, 750},
																	new[] {768, 855},
																	new[] {861, 879},
																	new[] {890, 890},
																	new[] {902, 902},
																	new[] {904, 906},
																	new[] {908, 908},
																	new[] {910, 929},
																	new[] {931, 974},
																	new[] {976, 1013},
																	new[] {1015, 1019},
																	new[] {1024, 1153},
																	new[] {1155, 1158},
																	new[] {1162, 1230},
																	new[] {1232, 1269},
																	new[] {1272, 1273},
																	new[] {1280, 1295},
																	new[] {1329, 1366},
																	new[] {1369, 1369},
																	new[] {1377, 1415},
																	new[] {1425, 1441},
																	new[] {1443, 1465},
																	new[] {1467, 1469},
																	new[] {1471, 1471},
																	new[] {1473, 1474},
																	new[] {1476, 1476},
																	new[] {1488, 1514},
																	new[] {1520, 1522},
																	new[] {1536, 1539},
																	new[] {1552, 1557},
																	new[] {1569, 1594},
																	new[] {1600, 1624},
																	new[] {1632, 1641},
																	new[] {1646, 1747},
																	new[] {1749, 1757},
																	new[] {1759, 1768},
																	new[] {1770, 1788},
																	new[] {1791, 1791},
																	new[] {1807, 1866},
																	new[] {1869, 1871},
																	new[] {1920, 1969},
																	new[] {2305, 2361},
																	new[] {2364, 2381},
																	new[] {2384, 2388},
																	new[] {2392, 2403},
																	new[] {2406, 2415},
																	new[] {2433, 2435},
																	new[] {2437, 2444},
																	new[] {2447, 2448},
																	new[] {2451, 2472},
																	new[] {2474, 2480},
																	new[] {2482, 2482},
																	new[] {2486, 2489},
																	new[] {2492, 2500},
																	new[] {2503, 2504},
																	new[] {2507, 2509},
																	new[] {2519, 2519},
																	new[] {2524, 2525},
																	new[] {2527, 2531},
																	new[] {2534, 2547},
																	new[] {2561, 2563},
																	new[] {2565, 2570},
																	new[] {2575, 2576},
																	new[] {2579, 2600},
																	new[] {2602, 2608},
																	new[] {2610, 2611},
																	new[] {2613, 2614},
																	new[] {2616, 2617},
																	new[] {2620, 2620},
																	new[] {2622, 2626},
																	new[] {2631, 2632},
																	new[] {2635, 2637},
																	new[] {2649, 2652},
																	new[] {2654, 2654},
																	new[] {2662, 2676},
																	new[] {2689, 2691},
																	new[] {2693, 2701},
																	new[] {2703, 2705},
																	new[] {2707, 2728},
																	new[] {2730, 2736},
																	new[] {2738, 2739},
																	new[] {2741, 2745},
																	new[] {2748, 2757},
																	new[] {2759, 2761},
																	new[] {2763, 2765},
																	new[] {2768, 2768},
																	new[] {2784, 2787},
																	new[] {2790, 2799},
																	new[] {2801, 2801},
																	new[] {2817, 2819},
																	new[] {2821, 2828},
																	new[] {2831, 2832},
																	new[] {2835, 2856},
																	new[] {2858, 2864},
																	new[] {2866, 2867},
																	new[] {2869, 2873},
																	new[] {2876, 2883},
																	new[] {2887, 2888},
																	new[] {2891, 2893},
																	new[] {2902, 2903},
																	new[] {2908, 2909},
																	new[] {2911, 2913},
																	new[] {2918, 2927},
																	new[] {2929, 2929},
																	new[] {2946, 2947},
																	new[] {2949, 2954},
																	new[] {2958, 2960},
																	new[] {2962, 2965},
																	new[] {2969, 2970},
																	new[] {2972, 2972},
																	new[] {2974, 2975},
																	new[] {2979, 2980},
																	new[] {2984, 2986},
																	new[] {2990, 2997},
																	new[] {2999, 3001},
																	new[] {3006, 3010},
																	new[] {3014, 3016},
																	new[] {3018, 3021},
																	new[] {3031, 3031},
																	new[] {3047, 3055},
																	new[] {3065, 3065},
																	new[] {3073, 3075},
																	new[] {3077, 3084},
																	new[] {3086, 3088},
																	new[] {3090, 3112},
																	new[] {3114, 3123},
																	new[] {3125, 3129},
																	new[] {3134, 3140},
																	new[] {3142, 3144},
																	new[] {3146, 3149},
																	new[] {3157, 3158},
																	new[] {3168, 3169},
																	new[] {3174, 3183},
																	new[] {3202, 3203},
																	new[] {3205, 3212},
																	new[] {3214, 3216},
																	new[] {3218, 3240},
																	new[] {3242, 3251},
																	new[] {3253, 3257},
																	new[] {3260, 3268},
																	new[] {3270, 3272},
																	new[] {3274, 3277},
																	new[] {3285, 3286},
																	new[] {3294, 3294},
																	new[] {3296, 3297},
																	new[] {3302, 3311},
																	new[] {3330, 3331},
																	new[] {3333, 3340},
																	new[] {3342, 3344},
																	new[] {3346, 3368},
																	new[] {3370, 3385},
																	new[] {3390, 3395},
																	new[] {3398, 3400},
																	new[] {3402, 3405},
																	new[] {3415, 3415},
																	new[] {3424, 3425},
																	new[] {3430, 3439},
																	new[] {3458, 3459},
																	new[] {3461, 3478},
																	new[] {3482, 3505},
																	new[] {3507, 3515},
																	new[] {3517, 3517},
																	new[] {3520, 3526},
																	new[] {3530, 3530},
																	new[] {3535, 3540},
																	new[] {3542, 3542},
																	new[] {3544, 3551},
																	new[] {3570, 3571},
																	new[] {3585, 3642},
																	new[] {3647, 3662},
																	new[] {3664, 3673},
																	new[] {3713, 3714},
																	new[] {3716, 3716},
																	new[] {3719, 3720},
																	new[] {3722, 3722},
																	new[] {3725, 3725},
																	new[] {3732, 3735},
																	new[] {3737, 3743},
																	new[] {3745, 3747},
																	new[] {3749, 3749},
																	new[] {3751, 3751},
																	new[] {3754, 3755},
																	new[] {3757, 3769},
																	new[] {3771, 3773},
																	new[] {3776, 3780},
																	new[] {3782, 3782},
																	new[] {3784, 3789},
																	new[] {3792, 3801},
																	new[] {3804, 3805},
																	new[] {3840, 3840},
																	new[] {3864, 3865},
																	new[] {3872, 3881},
																	new[] {3893, 3893},
																	new[] {3895, 3895},
																	new[] {3897, 3897},
																	new[] {3902, 3911},
																	new[] {3913, 3946},
																	new[] {3953, 3972},
																	new[] {3974, 3979},
																	new[] {3984, 3991},
																	new[] {3993, 4028},
																	new[] {4038, 4038},
																	new[] {4096, 4129},
																	new[] {4131, 4135},
																	new[] {4137, 4138},
																	new[] {4140, 4146},
																	new[] {4150, 4153},
																	new[] {4160, 4169},
																	new[] {4176, 4185},
																	new[] {4256, 4293},
																	new[] {4304, 4344},
																	new[] {4352, 4441},
																	new[] {4447, 4514},
																	new[] {4520, 4601},
																	new[] {4608, 4614},
																	new[] {4616, 4678},
																	new[] {4680, 4680},
																	new[] {4682, 4685},
																	new[] {4688, 4694},
																	new[] {4696, 4696},
																	new[] {4698, 4701},
																	new[] {4704, 4742},
																	new[] {4744, 4744},
																	new[] {4746, 4749},
																	new[] {4752, 4782},
																	new[] {4784, 4784},
																	new[] {4786, 4789},
																	new[] {4792, 4798},
																	new[] {4800, 4800},
																	new[] {4802, 4805},
																	new[] {4808, 4814},
																	new[] {4816, 4822},
																	new[] {4824, 4846},
																	new[] {4848, 4878},
																	new[] {4880, 4880},
																	new[] {4882, 4885},
																	new[] {4888, 4894},
																	new[] {4896, 4934},
																	new[] {4936, 4954},
																	new[] {4969, 4977},
																	new[] {5024, 5108},
																	new[] {5121, 5740},
																	new[] {5743, 5750},
																	new[] {5761, 5786},
																	new[] {5792, 5866},
																	new[] {5870, 5872},
																	new[] {5888, 5900},
																	new[] {5902, 5908},
																	new[] {5920, 5940},
																	new[] {5952, 5971},
																	new[] {5984, 5996},
																	new[] {5998, 6000},
																	new[] {6002, 6003},
																	new[] {6016, 6099},
																	new[] {6103, 6103},
																	new[] {6107, 6109},
																	new[] {6112, 6121},
																	new[] {6155, 6157},
																	new[] {6160, 6169},
																	new[] {6176, 6263},
																	new[] {6272, 6313},
																	new[] {6400, 6428},
																	new[] {6432, 6443},
																	new[] {6448, 6459},
																	new[] {6470, 6509},
																	new[] {6512, 6516},
																	new[] {7424, 7531},
																	new[] {7680, 7835},
																	new[] {7840, 7929},
																	new[] {7936, 7957},
																	new[] {7960, 7965},
																	new[] {7968, 8005},
																	new[] {8008, 8013},
																	new[] {8016, 8023},
																	new[] {8025, 8025},
																	new[] {8027, 8027},
																	new[] {8029, 8029},
																	new[] {8031, 8061},
																	new[] {8064, 8116},
																	new[] {8118, 8124},
																	new[] {8126, 8126},
																	new[] {8130, 8132},
																	new[] {8134, 8140},
																	new[] {8144, 8147},
																	new[] {8150, 8155},
																	new[] {8160, 8172},
																	new[] {8178, 8180},
																	new[] {8182, 8188},
																	new[] {8204, 8207},
																	new[] {8234, 8238},
																	new[] {8255, 8256},
																	new[] {8276, 8276},
																	new[] {8288, 8291},
																	new[] {8298, 8303},
																	new[] {8305, 8305},
																	new[] {8319, 8319},
																	new[] {8352, 8369},
																	new[] {8400, 8412},
																	new[] {8417, 8417},
																	new[] {8421, 8426},
																	new[] {8450, 8450},
																	new[] {8455, 8455},
																	new[] {8458, 8467},
																	new[] {8469, 8469},
																	new[] {8473, 8477},
																	new[] {8484, 8484},
																	new[] {8486, 8486},
																	new[] {8488, 8488},
																	new[] {8490, 8493},
																	new[] {8495, 8497},
																	new[] {8499, 8505},
																	new[] {8509, 8511},
																	new[] {8517, 8521},
																	new[] {8544, 8579},
																	new[] {12293, 12295},
																	new[] {12321, 12335},
																	new[] {12337, 12341},
																	new[] {12344, 12348},
																	new[] {12353, 12438},
																	new[] {12441, 12442},
																	new[] {12445, 12447},
																	new[] {12449, 12543},
																	new[] {12549, 12588},
																	new[] {12593, 12686},
																	new[] {12704, 12727},
																	new[] {12784, 12799},
																	new[] {13312, 19893},
																	new[] {19968, 40869},
																	new[] {40960, 42124},
																	new[] {44032, 55203},
																	new[] {63744, 64045},
																	new[] {64048, 64106},
																	new[] {64256, 64262},
																	new[] {64275, 64279},
																	new[] {64285, 64296},
																	new[] {64298, 64310},
																	new[] {64312, 64316},
																	new[] {64318, 64318},
																	new[] {64320, 64321},
																	new[] {64323, 64324},
																	new[] {64326, 64433},
																	new[] {64467, 64829},
																	new[] {64848, 64911},
																	new[] {64914, 64967},
																	new[] {65008, 65020},
																	new[] {65024, 65039},
																	new[] {65056, 65059},
																	new[] {65075, 65076},
																	new[] {65101, 65103},
																	new[] {65129, 65129},
																	new[] {65136, 65140},
																	new[] {65142, 65276},
																	new[] {65279, 65279},
																	new[] {65284, 65284},
																	new[] {65296, 65305},
																	new[] {65313, 65338},
																	new[] {65343, 65343},
																	new[] {65345, 65370},
																	new[] {65381, 65470},
																	new[] {65474, 65479},
																	new[] {65482, 65487},
																	new[] {65490, 65495},
																	new[] {65498, 65500},
																	new[] {65504, 65505},
																	new[] {65509, 65510},
																	new[] {65529, 65531},
#endregion
																};

		}
	}

partial class JavaGrammar
{
	private void InitializeSyntax()
	{
		bool enableAutomaticConflictResolution = true; //Roman: moved it here and made var instead of const to get rid of compiler warnings

		#region Identifier and Literals
#pragma warning disable 168
#pragma warning disable 162
		// ReSharper disable InconsistentNaming
		// ReSharper disable ConditionIsAlwaysTrueOrFalse

		var identifier_raw = new IdentifierTerminal("_identifier_")
		{
			AllFirstChars = ValidIdentifierStartCharacters,
			AllChars = ValidIdentifierCharacters
		};
		var identifier = new NonTerminal("identifier")
		{
			Rule = enableAutomaticConflictResolution
									? PreferShiftHere() + identifier_raw
									: identifier_raw
		};

		var number_literal = CreateJavaNumber("number");
		var character_literal = CreateJavaChar("char");
		var string_literal = CreateJavaString("string");
		var null_literal = CreateJavaNull("null");

		// ReSharper restore ConditionIsAlwaysTrueOrFalse
		// ReSharper restore InconsistentNaming
#pragma warning restore 162
#pragma warning restore 168
		#endregion

		#region Terminals
#pragma warning disable 168
		// ReSharper disable InconsistentNaming

		var ABSTRACT = ToTerm("abstract", "abstract");
		var AMP = ToTerm("&", "amp");
		var AMP_AMP = ToTerm("&&", "amp_amp");
		var AMP_ASSIGN = ToTerm("&=", "amp_assign");
		var ASSERT = ToTerm("assert", "assert");
		var ASSIGN = ToTerm("=", "assign");
		var AT = ToTerm("@", "at");
		var BAR = ToTerm("|", "bar");
		var BAR_ASSIGN = ToTerm("|=", "bar_assign");
		var BAR_BAR = ToTerm("||", "bar_bar");
		var BOOLEAN = ToTerm("boolean", "boolean");
		var BREAK = ToTerm("break", "break");
		var BYTE = ToTerm("byte", "byte");
		var CARET = ToTerm("^", "caret");
		var CARET_ASSIGN = ToTerm("^=", "caret_assign");
		var CASE = ToTerm("case", "case");
		var CATCH = ToTerm("catch", "catch");
		var CHAR = ToTerm("char", "char");
		var CLASS_TOKEN = ToTerm("class", "class_token");
		var COLON = ToTerm(":", "colon");
		var COMMA = ToTerm(",", "comma");
		var CONST = ToTerm("const", "const");
		var CONTINUE = ToTerm("continue", "continue");
		var DEFAULT = ToTerm("default", "default");
		var DO = ToTerm("do", "do");
		var DOT_DOT_DOT = ToTerm("...", "dot_dot_dot");
		var DOUBLE = ToTerm("double", "double");
		var ELSE = ToTerm("else", "else");
		var EMARK = ToTerm("!", "emark");
		var ENUM = ToTerm("enum", "enum");
		var EQ = ToTerm("==", "eq");
		var EXTENDS = ToTerm("extends", "extends");
		var FALSE = ToTerm("false", "false");
		var FINAL = ToTerm("final", "final");
		var FINALLY_TOKEN = ToTerm("finally", "finally_token");
		var FLOAT = ToTerm("float", "float");
		var FOR = ToTerm("for", "for");
		var GOTO = ToTerm("goto", "goto");
		var GT = ToTerm(">", "gt");
		var GTEQ = ToTerm(">=", "gteq");
		var IF = ToTerm("if", "if");
		var IMPLEMENTS = ToTerm("implements", "implements");
		var IMPORT = ToTerm("import", "import");
		var INSTANCEOF = ToTerm("instanceof", "instanceof");
		var INT = ToTerm("int", "int");
		var INTERFACE = ToTerm("interface", "interface");
		var L_BRC = ToTerm("{", "l_brc");
		var LONG = ToTerm("long", "long");
		var LTEQ = ToTerm("<=", "lteq");
		var MINUS = ToTerm("-", "minus");
		var MINUS_ASSIGN = ToTerm("-=", "minus_assign");
		var MINUS_MINUS = ToTerm("--", "minus_minus");
		var NATIVE = ToTerm("native", "native");
		var NEQ = ToTerm("!=", "neq");
		var NEW = ToTerm("new", "new");
		var PACKAGE = ToTerm("package", "package");
		var PERCENT = ToTerm("%", "percent");
		var PERCENT_ASSIGN = ToTerm("%=", "percent_assign");
		var PLUS = ToTerm("+", "plus");
		var PLUS_ASSIGN = ToTerm("+=", "plus_assign");
		var PLUS_PLUS = ToTerm("++", "plus_plus");
		var PRIVATE = ToTerm("private", "private");
		var PROTECTED = ToTerm("protected", "protected");
		var PUBLIC = ToTerm("public", "public");
		var QMARK = ToTerm("?", "qmark");
		var R_BKT = ToTerm("]", "r_bkt");
		var R_BRC = ToTerm("}", "r_brc");
		var R_PAR = ToTerm(")", "r_par");
		var RETURN = ToTerm("return", "return");
		var SEMI = ToTerm(";", "semi");
		var SHL = ToTerm("<<", "shl");
		var SHL_ASSIGN = ToTerm("<<=", "shl_assign");
		var SHORT = ToTerm("short", "short");
		var SHR = ToTerm(">>", "shr");
		var SHR_ASSIGN = ToTerm(">>=", "shr_assign");
		var SLASH = ToTerm("/", "slash");
		var SLASH_ASSIGN = ToTerm("/=", "slash_assign");
		var STAR = ToTerm("*", "star");
		var STAR_ASSIGN = ToTerm("*=", "star_assign");
		var STATIC = ToTerm("static", "static");
		var STRICTFP = ToTerm("strictfp", "strictfp");
		var SWITCH = ToTerm("switch", "switch");
		var SYNCHRONIZED = ToTerm("synchronized", "synchronized");
		var THROW = ToTerm("throw", "throw");
		var THROWS_TOKEN = ToTerm("throws", "throws_token");
		var TILDE = ToTerm("~", "tilde");
		var TRANSIENT = ToTerm("transient", "transient");
		var TRUE = ToTerm("true", "true");
		var TRY = ToTerm("try", "try");
		var USHR = ToTerm(">>>", "ushr");
		var USHR_ASSIGN = ToTerm(">>>=", "ushr_assign");
		var VOID = ToTerm("void", "void");
		var VOLATILE = ToTerm("volatile", "volatile");
		var WHILE = ToTerm("while", "while");

		#region Terminals with conflicts
#pragma warning disable 162
		// ReSharper disable ConditionIsAlwaysTrueOrFalse

		var THIS_RAW = ToTerm("this", "this");
		var THIS = new NonTerminal("_this_")
		{
			Rule = enableAutomaticConflictResolution
							  ? PreferShiftHere() + THIS_RAW
							  : THIS_RAW
		};

		var LT_RAW = ToTerm("<", "lt");
		var LT = new NonTerminal("_<_")
		{
			Rule = CustomActionHere(ResolveConflicts) + LT_RAW
		};

		var L_PAR_RAW = ToTerm("(", "l_par");
		var L_PAR = new NonTerminal("_(_")
		{
			Rule = enableAutomaticConflictResolution
								? PreferShiftHere() + L_PAR_RAW
								: L_PAR_RAW
		};

		var L_BKT_RAW = ToTerm("[", "l_bkt");
		var L_BKT = new NonTerminal("_[_")
		{
			Rule = enableAutomaticConflictResolution
								? PreferShiftHere() + L_BKT_RAW
								: L_BKT_RAW
		};

		var DOT_RAW = ToTerm(".", "dot");
		var DOT = new NonTerminal("_._")
		{
			Rule = CustomActionHere(ResolveConflicts) + DOT_RAW
		};
		var SUPER_TOKEN_RAW = ToTerm("super", "super_token");
		var SUPER_TOKEN = new NonTerminal("_super_")
		{
			Rule = CustomActionHere(ResolveConflicts) + SUPER_TOKEN_RAW
		};

		// ReSharper restore ConditionIsAlwaysTrueOrFalse
#pragma warning restore 162
		#endregion

		// ReSharper restore InconsistentNaming
#pragma warning restore 168
		#endregion

		#region NonTerminal Declarations
		// ReSharper disable InconsistentNaming
		var abstract_method_declaration = new NonTerminal("abstract_method_declaration");
		var annotation = new NonTerminal("annotation");
		var annotation_declaration = new NonTerminal("annotation_declaration");
		var annotation_type_body = new NonTerminal("annotation_type_body");
		var annotation_type_element_declaration = new NonTerminal("annotation_type_element_declaration");
		var annotation_type_element_rest = new NonTerminal("annotation_type_element_rest");
		var annotations = new NonTerminal("annotations");
		var argument_list = new NonTerminal("argument_list");
		var arguments = new NonTerminal("arguments");
		var arguments_opt = new NonTerminal("arguments?");
		var array_access = new NonTerminal("array_access");
		var array_creator_rest = new NonTerminal("array_creator_rest");
		var array_initializer = new NonTerminal("array_initializer");
		var assignment_expression = new NonTerminal("assignment_expression");
		var assignment_operator = new NonTerminal("assignment_operator");
		var base_type_declaration = new NonTerminal("type_declaration_without_modifiers");
		var binary_expression = new NonTerminal("binary_expression");
		var block = new NonTerminal("block");
		var block_statement = new NonTerminal("block_statement");
		var block_statements = new NonTerminal("block_statements");
		var boolean_literal = new NonTerminal("boolean_literal");
		var cast_expression = new NonTerminal("cast_expression");
		var catch_clause = new NonTerminal("catch_clause");
		var catches = new NonTerminal("catches");
		var class_body = new NonTerminal("class_body");
		var class_body_declaration = new NonTerminal("class_body_declaration");
		var class_body_opt = new NonTerminal("class_body_opt");
		var class_creator_rest = new NonTerminal("class_creator_rest");
		var class_declaration = new NonTerminal("class_declaration");
		var class_member_declaration = new NonTerminal("class_member_declaration");
		var compilation_unit = new NonTerminal("compilation_unit");
		var constant_declaration = new NonTerminal("constant_declaration");
		var constructor_body = new NonTerminal("constructor_body");
		var constructor_declaration = new NonTerminal("constructor_declaration");
		var created_name = new NonTerminal("created_name");
		var creator = new NonTerminal("creator");
		var dim = new NonTerminal("dim");
		var dim_expr = new NonTerminal("dim_expr");
		var dim_exprs = new NonTerminal("dims_exprs");
		var dims = new NonTerminal("dims");
		var element_value = new NonTerminal("element_value");
		var element_value_array_initializer = new NonTerminal("element_value_array_initializer");
		var element_value_pair = new NonTerminal("element_value_pair");
		var element_value_pairs = new NonTerminal("element_value_pairs");
		var element_values = new NonTerminal("element_values");
		var enum_body = new NonTerminal("enum_body");
		var enum_body_declarations = new NonTerminal("enum_body_declarations");
		var enum_body_declarations_opt = new NonTerminal("enum_body_declarations?");
		var enum_constant = new NonTerminal("enum_constant");
		var enum_constants = new NonTerminal("enum_constants");
		var enum_declaration = new NonTerminal("enum_declaration");
		var exception_type_list = new NonTerminal("exception_type_list");
		var explicit_constructor_invocation = new NonTerminal("explicit_constructor_invocation");
		var explicit_generic_invocation = new NonTerminal("explicit_generic_invocation");
		var explicit_generic_invocation_suffix = new NonTerminal("explicit_generic_invocation_suffix");
		var expression = new NonTerminal("expression");
		var expression_in_parens = new NonTerminal("expression_in_parens");
		var field_access = new NonTerminal("field_access");
		var field_declaration = new NonTerminal("field_declaration");
		var for_control = new NonTerminal("for_control");
		var for_init = new NonTerminal("for_init");
		var for_update = new NonTerminal("for_update");
		var for_var_control = new NonTerminal("for_var_control");
		var formal_parameter = new NonTerminal("formal_parameter");
		var formal_parameter_list = new NonTerminal("formal_parameter_list");
		var formal_parameter_list_opt = new NonTerminal("formal_parameter_list?");
		var formal_parameters = new NonTerminal("formal_parameters");
		var identifier_suffix = new NonTerminal("identifier_suffix");
		var identifier_suffix_opt = new NonTerminal("identifier_suffix_opt");
		var import_declaration = new NonTerminal("import_declaration");
		var import_declarations = new NonTerminal("import_declarations");
		var import_wildcard = new NonTerminal("import_wildcard");
		var infix_operator = new NonTerminal("infix_operator");
		var inner_creator = new NonTerminal("inner_creator");
		var instance_initializer = new NonTerminal("instance_initializer");
		var interface_body = new NonTerminal("interface_body");
		var interface_declaration = new NonTerminal("interface_declaration");
		var interface_member_declaration = new NonTerminal("interface_member_declaration");
		var interface_type = new NonTerminal("interface_type");
		var interface_type_list = new NonTerminal("interface_type_list");
		var interfaces = new NonTerminal("interfaces");
		var interfaces_opt = new NonTerminal("interfaces?");
		var last_formal_parameter = new NonTerminal("last_formal_parameter");
		var left_hand_side = new NonTerminal("left_hand_side");
		var literal = new NonTerminal("literal");
		var local_variable_declaration = new NonTerminal("local_variable_declaration_statement");
		var method_body = new NonTerminal("method_body");
		var method_declaration = new NonTerminal("method_declaration");
		var method_declarator = new NonTerminal("method_declarator");
		var method_invocation = new NonTerminal("method_invocation");
		var modifier = new NonTerminal("modifier");
		var modifiers = new NonTerminal("modifiers");
		var modifiers_opt = new NonTerminal("modifiers?");
		var normal_import_declaration = new NonTerminal("normal_import_declaration");
		var package_declaration = new NonTerminal("package_declaration");
		var package_declaration_w_modifiers = new NonTerminal("package_declaration");
		var postfix_operator = new NonTerminal("postfix_operator");
		var prefix_operator = new NonTerminal("prefix_operator");
		var primary_expression = new NonTerminal("primary_expression");
		var primary_expression_no_new = new NonTerminal("primary_expression_no_new");
		var primitive_type = new NonTerminal("primitive_type");
		var qualified_name = new NonTerminal("qualified_name");
		var selector = new NonTerminal("selector");
		var statement = new NonTerminal("statement");
		var statement_expression = new NonTerminal("statement_expression");
		var static_import_declaration = new NonTerminal("static_import_declaration");
		var static_initializer = new NonTerminal("static_initializer");
		var super = new NonTerminal("super");
		var super_opt = new NonTerminal("super?");
		var super_suffix = new NonTerminal("super_suffix");
		var switch_block_statement_group = new NonTerminal("switch_block_statement_group");
		var switch_block_statement_groups = new NonTerminal("switch_block_statement_groups");
		var switch_label = new NonTerminal("switch_label");
		var templated_identifier = new NonTerminal("templated_identifier");
		var templated_qualified_name = new NonTerminal("templated_qualified_name");
		var throws = new NonTerminal("throws");
		var throws_opt = new NonTerminal("throws?");
		var trinary_expression = new NonTerminal("trinary_expression");
		var type = new NonTerminal("type");
		var type_argument = new NonTerminal("type_argument");
		var type_argument_list = new NonTerminal("type_argument_list");
		var type_arguments = new NonTerminal("type_arguments");
		var type_arguments_opt = new NonTerminal("type_arguments?");
		var type_bound = new NonTerminal("type_bound");
		var type_bound_list = new NonTerminal("type_bound_list");
		var type_bound_opt = new NonTerminal("type_bound?");
		var type_declaration = new NonTerminal("type_declaration");
		var type_declaration_w_modifiers = new NonTerminal("type_declaration_with_modifiers");
		var type_declarations = new NonTerminal("type_declarations");
		var type_parameter = new NonTerminal("type_parameter");
		var type_parameter_list = new NonTerminal("type_parameter_list");
		var type_parameters = new NonTerminal("type_parameters");
		var type_parameters_opt = new NonTerminal("type_parameters?");
		var unary_expression = new NonTerminal("unary_expression");
		var variable_declarator = new NonTerminal("variable_declarator");
		var variable_declarators = new NonTerminal("variable_declarators");
		var variable_declarators_rest = new NonTerminal("variable_declarators_rest");
		var variable_initializer = new NonTerminal("variable_initializer");
		var variable_initializers = new NonTerminal("variable_initializers");
		// ReSharper restore InconsistentNaming
		#endregion

		#region NonTerminal Rules

		#region common
		modifiers_opt.Rule = Empty | modifiers;
		modifiers.Rule = MakePlusRule(modifiers, modifier);
		modifier.Rule = ABSTRACT | FINAL | NATIVE | PRIVATE | PROTECTED | PUBLIC | STATIC | STRICTFP | SYNCHRONIZED | TRANSIENT | VOLATILE | annotation;
		qualified_name.Rule = MakePlusRule(qualified_name, DOT, identifier);
		type.Rule = templated_qualified_name;
		type.Rule |= templated_qualified_name + dim + dims;
		type.Rule |= primitive_type;
		type.Rule |= primitive_type + dim + dims;

		super_opt.Rule = Empty | super;
		super.Rule = EXTENDS + templated_qualified_name;

		interfaces_opt.Rule = Empty | interfaces;
		interfaces.Rule = IMPLEMENTS + interface_type_list;
		interface_type_list.Rule = MakePlusRule(interface_type_list, COMMA, interface_type);
		interface_type.Rule = templated_qualified_name;

		templated_identifier.Rule = identifier + type_arguments;
		templated_identifier.Rule |= identifier;
		templated_qualified_name.Rule = MakePlusRule(templated_qualified_name, DOT, templated_identifier);

		type_arguments_opt.Rule = Empty | type_arguments;
		type_arguments.Rule = LT + type_argument_list + GT;
		type_argument_list.Rule = MakePlusRule(type_argument_list, COMMA, type_argument);
		type_argument.Rule = type;
		type_argument.Rule |= QMARK + EXTENDS + type;
		type_argument.Rule |= QMARK + SUPER_TOKEN + type;
		type_argument.Rule |= QMARK;

		type_parameters_opt.Rule = Empty | type_parameters;
		type_parameters.Rule = LT + type_parameter_list + GT;
		type_parameter_list.Rule = MakePlusRule(type_parameter_list, COMMA, type_parameter);
		type_parameter.Rule = type;
		type_parameter.Rule |= type + type_bound;
		type_bound_opt.Rule = Empty | type_bound;
		type_bound.Rule = EXTENDS + type_bound_list;
		type_bound_list.Rule = MakePlusRule(type_bound_list, AMP, interface_type);

		dims.Rule = MakeStarRule(dims, dim);
		dim.Rule = L_BKT + R_BKT;

		dim_exprs.Rule = MakePlusRule(dim_exprs, dim_expr);
		dim_expr.Rule = L_BKT + expression + R_BKT;

		method_declarator.Rule = identifier + L_PAR + formal_parameter_list_opt + R_PAR + dims;
		throws_opt.Rule = Empty | throws;
		throws.Rule = THROWS_TOKEN + exception_type_list;
		exception_type_list.Rule = MakePlusRule(exception_type_list, COMMA, templated_qualified_name);

		formal_parameter_list_opt.Rule = Empty | formal_parameter_list;
		formal_parameter_list.Rule = formal_parameters + COMMA + last_formal_parameter;
		formal_parameter_list.Rule |= last_formal_parameter;
		formal_parameters.Rule = MakePlusRule(formal_parameters, COMMA, formal_parameter);
		formal_parameter.Rule = modifiers_opt + primitive_type + dims + identifier + dims;
		formal_parameter.Rule |= modifiers_opt + templated_qualified_name + dims + identifier + dims;
		last_formal_parameter.Rule = formal_parameter;
		last_formal_parameter.Rule |= modifiers_opt + primitive_type + dims + DOT_DOT_DOT + identifier + dims;
		last_formal_parameter.Rule |= modifiers_opt + templated_qualified_name + dims + DOT_DOT_DOT + identifier + dims;

		variable_declarators_rest.Rule = MakeStarRule(variable_declarators_rest, COMMA + variable_declarator);
		variable_declarators.Rule = MakePlusRule(variable_declarators, COMMA, variable_declarator);
		variable_declarator.Rule = templated_qualified_name + dims;
		variable_declarator.Rule |= templated_qualified_name + dims + ASSIGN + variable_initializer;

		//variable_initializers.Rule = MakeStarRule(variable_initializers, COMMA, variable_initializer, TermListOptions.AllowTrailingDelimiter);
		variable_initializers.Rule = MakeListRule(variable_initializers, COMMA, variable_initializer, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
		variable_initializer.Rule = array_initializer;
		variable_initializer.Rule |= expression;

		array_initializer.Rule = L_BRC + variable_initializers + R_BRC;
		array_initializer.Rule |= L_BRC + COMMA + R_BRC;

		primitive_type.Rule = BOOLEAN;
		primitive_type.Rule |= BYTE;
		primitive_type.Rule |= SHORT;
		primitive_type.Rule |= INT;
		primitive_type.Rule |= LONG;
		primitive_type.Rule |= CHAR;
		primitive_type.Rule |= FLOAT;
		primitive_type.Rule |= DOUBLE;

		literal.Rule = null_literal;
		literal.Rule |= number_literal;
		literal.Rule |= character_literal;
		literal.Rule |= string_literal;
		literal.Rule |= boolean_literal;

		boolean_literal.Rule = TRUE | FALSE;

		statement_expression.Rule = expression;

		block.Rule = L_BRC + block_statements + R_BRC;
		block_statements.Rule = MakeStarRule(block_statements, block_statement);
		block_statement.Rule = statement;
		block_statement.Rule |= explicit_constructor_invocation;
		block_statement.Rule |= modifiers + local_variable_declaration;
		block_statement.Rule |= local_variable_declaration;
		block_statement.Rule |= modifiers + annotation_declaration;
		block_statement.Rule |= annotation_declaration;
		block_statement.Rule |= modifiers + class_declaration;
		block_statement.Rule |= class_declaration;
		block_statement.Rule |= modifiers + enum_declaration;
		block_statement.Rule |= enum_declaration;
		block_statement.Rule |= modifiers + interface_declaration;
		block_statement.Rule |= interface_declaration;

		local_variable_declaration.Rule = primitive_type + dim + dims + variable_declarators + SEMI;
		local_variable_declaration.Rule |= primitive_type + variable_declarators + SEMI;
		local_variable_declaration.Rule |= templated_qualified_name + dim + dims + variable_declarators + SEMI;
		local_variable_declaration.Rule |= templated_qualified_name + variable_declarators + SEMI;
		#endregion

		#region top level
		compilation_unit.Rule = Empty;
		compilation_unit.Rule |= package_declaration_w_modifiers + import_declarations + type_declarations;
		compilation_unit.Rule |= package_declaration + import_declarations + type_declarations;
		compilation_unit.Rule |= import_declarations + type_declarations;
		compilation_unit.Rule |= package_declaration_w_modifiers + type_declarations;
		compilation_unit.Rule |= package_declaration + type_declarations;
		compilation_unit.Rule |= type_declarations;
		compilation_unit.Rule |= package_declaration_w_modifiers + import_declarations;
		compilation_unit.Rule |= package_declaration + import_declarations;
		compilation_unit.Rule |= import_declarations;
		compilation_unit.Rule |= package_declaration_w_modifiers;
		compilation_unit.Rule |= package_declaration;

		package_declaration.Rule = PACKAGE + qualified_name + SEMI;
		package_declaration_w_modifiers.Rule = modifiers + PACKAGE + qualified_name + SEMI;

		import_declarations.Rule = MakePlusRule(import_declarations, import_declaration);
		import_declaration.Rule = normal_import_declaration | static_import_declaration;
		normal_import_declaration.Rule = IMPORT + qualified_name + import_wildcard + SEMI;
		static_import_declaration.Rule = IMPORT + STATIC + qualified_name + import_wildcard + SEMI;
		import_wildcard.Rule = Empty | (DOT + STAR);

		type_declarations.Rule = MakePlusRule(type_declarations, type_declaration);
		type_declaration.Rule = type_declaration_w_modifiers | base_type_declaration | SEMI;

		type_declaration_w_modifiers.Rule = modifiers + base_type_declaration;
		base_type_declaration.Rule = annotation_declaration | class_declaration | enum_declaration | interface_declaration;
		#endregion

		#region type declarations
		annotation_declaration.Rule = AT + INTERFACE + identifier + L_BRC + annotation_type_body + R_BRC;
		class_declaration.Rule = CLASS_TOKEN + identifier + type_parameters_opt + super_opt + interfaces_opt + L_BRC + class_body + R_BRC;
		enum_declaration.Rule = ENUM + identifier + interfaces_opt + L_BRC + enum_body + R_BRC;
		interface_declaration.Rule = INTERFACE + identifier + type_parameters_opt + EXTENDS + interface_type_list + L_BRC + interface_body + R_BRC;
		interface_declaration.Rule |= INTERFACE + identifier + type_parameters_opt + L_BRC + interface_body + R_BRC;
		#endregion

		#region interface_declaration
		interface_body.Rule = MakeStarRule(interface_body, interface_member_declaration);
		interface_member_declaration.Rule = SEMI;
		interface_member_declaration.Rule |= modifiers_opt + constant_declaration;
		interface_member_declaration.Rule |= modifiers_opt + abstract_method_declaration;
		interface_member_declaration.Rule |= modifiers_opt + class_declaration;
		interface_member_declaration.Rule |= modifiers_opt + interface_declaration;

		// these type_parameters are not actually allowed but it saves a whole lot of work in resolving code.
		constant_declaration.Rule = type_parameters_opt + primitive_type + dims + variable_declarators + SEMI;
		constant_declaration.Rule |= type_parameters_opt + templated_qualified_name + dims + variable_declarators + SEMI;

		abstract_method_declaration.Rule = type_parameters_opt + primitive_type + dims + method_declarator + throws_opt + SEMI;
		abstract_method_declaration.Rule |= type_parameters_opt + templated_qualified_name + dims + method_declarator + throws_opt + SEMI;
		abstract_method_declaration.Rule |= type_parameters_opt + VOID + dims + method_declarator + throws_opt + SEMI;
		#endregion

		#region class_declaration
		class_body_opt.Rule = Empty | L_BRC + class_body + R_BRC;
		class_body.Rule = MakeStarRule(class_body, class_body_declaration);

		class_body_declaration.Rule = class_member_declaration;
		class_body_declaration.Rule |= instance_initializer;
		class_body_declaration.Rule |= static_initializer;
		class_body_declaration.Rule |= constructor_declaration;

		class_member_declaration.Rule = SEMI;
		class_member_declaration.Rule |= modifiers + field_declaration;
		class_member_declaration.Rule |= field_declaration;
		class_member_declaration.Rule |= modifiers + method_declaration;
		class_member_declaration.Rule |= method_declaration;
		class_member_declaration.Rule |= modifiers + annotation_declaration;
		class_member_declaration.Rule |= annotation_declaration;
		class_member_declaration.Rule |= modifiers + class_declaration;
		class_member_declaration.Rule |= class_declaration;
		class_member_declaration.Rule |= modifiers + enum_declaration;
		class_member_declaration.Rule |= enum_declaration;
		class_member_declaration.Rule |= modifiers + interface_declaration;
		class_member_declaration.Rule |= interface_declaration;

		// these type_parameters are not actually allowed but it saves a whole lot of work in resolving code.
		field_declaration.Rule = type_parameters_opt + primitive_type + dims + variable_declarators + SEMI;
		field_declaration.Rule |= type_parameters_opt + templated_qualified_name + dims + variable_declarators + SEMI;

		method_declaration.Rule = type_parameters_opt + VOID + method_declarator + throws_opt + method_body;
		method_declaration.Rule |= type_parameters_opt + primitive_type + dims + method_declarator + throws_opt + method_body;
		method_declaration.Rule |= type_parameters_opt + templated_qualified_name + dims + method_declarator + throws_opt + method_body;

		method_body.Rule = SEMI;
		method_body.Rule |= block;

		instance_initializer.Rule = block;
		static_initializer.Rule = STATIC + block;

		constructor_declaration.Rule = modifiers + type_parameters_opt + identifier + L_PAR + formal_parameter_list_opt + R_PAR + throws_opt + constructor_body;
		constructor_declaration.Rule |= type_parameters_opt + identifier + L_PAR + formal_parameter_list_opt + R_PAR + throws_opt + constructor_body;

		constructor_body.Rule = L_BRC + block_statements + R_BRC;

		explicit_constructor_invocation.Rule = type_arguments_opt + THIS + arguments + SEMI;
		explicit_constructor_invocation.Rule |= type_arguments_opt + SUPER_TOKEN + arguments + SEMI;
		#endregion

		#region annotations
		//element_value_pairs.Rule = MakeStarRule(element_value_pairs, COMMA, element_value_pair, TermListOptions.AllowTrailingDelimiter);
		element_value_pairs.Rule = MakeListRule(element_value_pairs, COMMA, element_value_pair, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
		element_value_pair.Rule = identifier + ASSIGN + element_value;
		element_value_array_initializer.Rule = L_BRC + element_values + R_BRC;
		//element_values.Rule = MakeStarRule(element_values, COMMA, element_value, TermListOptions.AllowTrailingDelimiter);
		element_values.Rule = MakeListRule(element_values, COMMA, element_value, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
		element_value.Rule = annotation;
		element_value.Rule |= element_value_array_initializer;
		element_value.Rule |= expression;

		annotations.Rule = MakeStarRule(annotations, annotation);
		annotation.Rule = AT + qualified_name + L_PAR + element_value_pair + R_PAR;
		annotation.Rule |= AT + qualified_name + L_PAR + element_value_pair + COMMA + element_value_pairs + R_PAR;
		annotation.Rule |= AT + qualified_name + L_PAR + element_values + R_PAR;
		annotation.Rule |= AT + qualified_name;


		annotation_type_body.Rule = MakeStarRule(annotation_type_body, annotation_type_element_declaration);
		annotation_type_element_declaration.Rule = modifiers + annotation_type_element_rest;
		annotation_type_element_declaration.Rule |= annotation_type_element_rest;

		annotation_type_element_rest.Rule = type + identifier + L_PAR + R_PAR + DEFAULT + element_value + SEMI;
		annotation_type_element_rest.Rule |= type + identifier + L_PAR + R_PAR + SEMI;
		annotation_type_element_rest.Rule |= type + variable_declarators + SEMI;
		annotation_type_element_rest.Rule |= class_declaration;
		annotation_type_element_rest.Rule |= enum_declaration;
		annotation_type_element_rest.Rule |= annotation_declaration;
		#endregion

		#region enum
		enum_body.Rule = enum_constants + enum_body_declarations_opt;
		enum_body.Rule |= COMMA + enum_body_declarations_opt;
		//enum_constants.Rule = MakeStarRule(enum_constants, COMMA, enum_constant, TermListOptions.AllowTrailingDelimiter);
		enum_constants.Rule = MakeListRule(enum_constants, COMMA, enum_constant, TermListOptions.StarList | TermListOptions.AllowTrailingDelimiter);
		enum_constant.Rule = annotations + identifier;
		enum_constant.Rule |= annotations + identifier + arguments;
		enum_constant.Rule |= annotations + identifier + arguments + L_BRC + class_body + R_BRC;
		enum_constant.Rule |= annotations + identifier + L_BRC + class_body + R_BRC;

		arguments_opt.Rule = Empty | arguments;
		arguments.Rule = L_PAR + argument_list + R_PAR;
		argument_list.Rule = MakeStarRule(argument_list, COMMA, expression);

		enum_body_declarations_opt.Rule = Empty | enum_body_declarations;
		enum_body_declarations.Rule = SEMI + class_body;
		#endregion

		#region expressions
		super_suffix.Rule = arguments;
		super_suffix.Rule |= DOT + identifier + arguments_opt;

		explicit_generic_invocation_suffix.Rule = SUPER_TOKEN + super_suffix;
		explicit_generic_invocation_suffix.Rule |= identifier + arguments;

		array_creator_rest.Rule = dim + dims + array_initializer;
		array_creator_rest.Rule |= dim_exprs + dim + dims;
		array_creator_rest.Rule |= dim_exprs;

		class_creator_rest.Rule = arguments + class_body_opt;

		creator.Rule = type_arguments_opt + created_name + (array_creator_rest | class_creator_rest);
		created_name.Rule = templated_qualified_name;

		explicit_generic_invocation.Rule = type_arguments + PreferShiftHere() + explicit_generic_invocation_suffix;

		inner_creator.Rule = templated_qualified_name + class_creator_rest;

		identifier_suffix_opt.Rule = Empty | identifier_suffix;
		identifier_suffix.Rule = dim + dims + DOT + CLASS_TOKEN;
		identifier_suffix.Rule = arguments;
		identifier_suffix.Rule = DOT + CLASS_TOKEN;
		identifier_suffix.Rule |= DOT + explicit_generic_invocation;
		identifier_suffix.Rule |= DOT + THIS;
		identifier_suffix.Rule |= DOT + type_arguments_opt + SUPER_TOKEN + arguments;
		identifier_suffix.Rule |= DOT + NEW + type_arguments_opt + inner_creator;

		selector.Rule = DOT + explicit_generic_invocation;
		selector.Rule |= DOT + THIS;
		selector.Rule |= DOT + type_arguments_opt + SUPER_TOKEN + arguments;
		selector.Rule |= DOT + NEW + type_arguments_opt + inner_creator;
		selector.Rule |= L_BKT + expression + R_BKT;

		primary_expression.Rule = primary_expression_no_new;
		primary_expression.Rule |= NEW + creator;

		expression_in_parens.Rule = L_PAR + expression + R_PAR;
		expression_in_parens.Rule |= L_PAR + templated_qualified_name + R_PAR;

		cast_expression.Rule = L_PAR + templated_qualified_name + R_PAR + expression;
		cast_expression.Rule |= L_PAR + templated_qualified_name + dim + dims + R_PAR + expression;
		cast_expression.Rule |= L_PAR + primitive_type + R_PAR + expression;
		cast_expression.Rule |= L_PAR + primitive_type + dim + dims + R_PAR + expression;

		primary_expression_no_new.Rule = expression_in_parens;
		primary_expression_no_new.Rule |= cast_expression;
		primary_expression_no_new.Rule |= THIS;
		primary_expression_no_new.Rule |= literal;
		primary_expression_no_new.Rule |= templated_qualified_name + dim + dims + identifier_suffix;
		primary_expression_no_new.Rule |= templated_qualified_name + dim + dims;
		primary_expression_no_new.Rule |= templated_qualified_name + identifier_suffix;
		primary_expression_no_new.Rule |= templated_qualified_name;
		primary_expression_no_new.Rule |= primitive_type + dims + DOT + CLASS_TOKEN;
		primary_expression_no_new.Rule |= VOID + DOT + CLASS_TOKEN;
		primary_expression_no_new.Rule |= array_access;
		primary_expression_no_new.Rule |= field_access;
		primary_expression_no_new.Rule |= method_invocation;

		method_invocation.Rule = expression + arguments;
		method_invocation.Rule |= expression + DOT + type_arguments_opt + identifier + arguments;
		method_invocation.Rule |= type_arguments_opt + SUPER_TOKEN + DOT + type_arguments_opt + identifier + arguments;
		method_invocation.Rule |= expression + DOT + type_arguments_opt + SUPER_TOKEN + DOT + type_arguments_opt + identifier + arguments;
		method_invocation.Rule |= type_arguments_opt + THIS + DOT + type_arguments_opt + identifier + arguments;
		method_invocation.Rule |= expression + DOT + type_arguments_opt + THIS + DOT + type_arguments_opt + identifier + arguments;

		field_access.Rule = expression + DOT + type_arguments_opt + identifier;
		field_access.Rule |= type_arguments_opt + SUPER_TOKEN + DOT + type_arguments_opt + identifier;
		field_access.Rule |= expression + DOT + type_arguments_opt + SUPER_TOKEN + DOT + type_arguments_opt + identifier;
		field_access.Rule |= type_arguments_opt + THIS + DOT + type_arguments_opt + identifier;
		field_access.Rule |= expression + DOT + type_arguments_opt + THIS + DOT + type_arguments_opt + identifier;

		assignment_operator.Rule = ASSIGN | PLUS_ASSIGN | MINUS_ASSIGN | STAR_ASSIGN | SLASH_ASSIGN | AMP_ASSIGN |
					  BAR_ASSIGN | CARET_ASSIGN | PERCENT_ASSIGN | SHL_ASSIGN | SHR_ASSIGN | USHR_ASSIGN;


		infix_operator.Rule = BAR_BAR | AMP_AMP | BAR | AMP | CARET | EQ | NEQ | LT | GT | LTEQ | GTEQ | SHR | SHL | USHR |
							  PLUS | MINUS | STAR | SLASH | PERCENT | INSTANCEOF;

		prefix_operator.Rule = PLUS_PLUS | MINUS_MINUS | EMARK | TILDE | PLUS | MINUS;
		postfix_operator.Rule = PLUS_PLUS | MINUS_MINUS;

		array_access.Rule = templated_qualified_name + dim_expr;
		array_access.Rule |= primary_expression_no_new + dim_expr;
		array_access.Rule |= array_access + dim_expr;

		left_hand_side.Rule = templated_qualified_name;
		left_hand_side.Rule |= expression_in_parens;
		left_hand_side.Rule |= field_access;
		left_hand_side.Rule |= array_access;

		unary_expression.Rule = prefix_operator + expression;
		unary_expression.Rule |= expression + selector + postfix_operator;
		unary_expression.Rule |= expression + selector;
		unary_expression.Rule |= expression + postfix_operator;

		binary_expression.Rule = expression + infix_operator + expression;
		trinary_expression.Rule = expression + QMARK + expression + COLON + expression;

		assignment_expression.Rule = left_hand_side + assignment_operator + expression;

		expression.Rule = primary_expression;
		expression.Rule |= assignment_expression;
		expression.Rule |= unary_expression;
		expression.Rule |= binary_expression;
		expression.Rule |= trinary_expression;
		#endregion

		#region statements
		statement.Rule = block;
		statement.Rule |= ASSERT + expression + SEMI;
		statement.Rule |= ASSERT + expression + COLON + expression + SEMI;
		statement.Rule |= IF + L_PAR + expression + R_PAR + statement;
		statement.Rule |= IF + L_PAR + expression + R_PAR + statement + PreferShiftHere() + ELSE + statement;
		statement.Rule |= FOR + L_PAR + for_control + R_PAR + statement;
		statement.Rule |= WHILE + L_PAR + expression + R_PAR + statement;
		statement.Rule |= DO + statement + WHILE + L_PAR + expression + R_PAR + SEMI;
		statement.Rule |= TRY + block + catches + FINALLY_TOKEN + block;
		statement.Rule |= TRY + block + catches;
		statement.Rule |= TRY + block + FINALLY_TOKEN + block;
		statement.Rule |= SWITCH + L_PAR + expression + R_PAR + L_BRC + switch_block_statement_groups + R_BRC;
		statement.Rule |= SYNCHRONIZED + L_PAR + expression + R_PAR + block;
		statement.Rule |= RETURN + expression + SEMI;
		statement.Rule |= RETURN + SEMI;
		statement.Rule |= THROW + expression + SEMI;
		statement.Rule |= BREAK + identifier + SEMI;
		statement.Rule |= BREAK + SEMI;
		statement.Rule |= CONTINUE + identifier + SEMI;
		statement.Rule |= CONTINUE + SEMI;
		statement.Rule |= SEMI;
		statement.Rule |= statement_expression + SEMI;
		statement.Rule |= identifier + COLON + statement;

		for_control.Rule = for_var_control;
		for_control.Rule |= for_init + SEMI + expression + SEMI + for_update;
		for_control.Rule |= for_init + SEMI + expression + SEMI;
		for_control.Rule |= for_init + SEMI + SEMI + for_update;
		for_control.Rule |= for_init + SEMI + SEMI;

		for_var_control.Rule = modifiers + type + identifier + COLON + expression;
		for_var_control.Rule |= type + identifier + COLON + expression;

		for_init.Rule = for_update;
		for_init.Rule |= modifiers + type + variable_declarators;
		for_init.Rule |= type + variable_declarators;

		for_update.Rule = MakePlusRule(for_update, COMMA, statement_expression);

		catches.Rule = MakePlusRule(catches, catch_clause);
		catch_clause.Rule = CATCH + L_PAR + formal_parameter + R_PAR + block;

		switch_block_statement_groups.Rule = MakeStarRule(switch_block_statement_groups, switch_block_statement_group);
		switch_block_statement_group.Rule = switch_label + block_statements;
		switch_label.Rule = CASE + expression + COLON;
		switch_label.Rule |= DEFAULT + COLON;
		#endregion

		#region operator precedence
		RegisterOperators(1, Associativity.Right, ASSIGN, PLUS_ASSIGN, MINUS_ASSIGN, STAR_ASSIGN, SLASH_ASSIGN, AMP_ASSIGN, BAR_ASSIGN, CARET_ASSIGN, PERCENT_ASSIGN, SHL_ASSIGN, SHR_ASSIGN, USHR_ASSIGN);
		RegisterOperators(2, Associativity.Right, QMARK);
		RegisterOperators(3, Associativity.Left, BAR_BAR);
		RegisterOperators(4, Associativity.Left, AMP_AMP);
		RegisterOperators(5, Associativity.Left, BAR);
		RegisterOperators(6, Associativity.Left, CARET);
		RegisterOperators(7, Associativity.Left, AMP);
		RegisterOperators(8, Associativity.Left, EQ, NEQ);
		RegisterOperators(9, Associativity.Left, INSTANCEOF, GT, GTEQ, LT, LTEQ);
		RegisterOperators(10, Associativity.Left, SHL, SHR, USHR);
		RegisterOperators(11, Associativity.Left, PLUS, MINUS);
		RegisterOperators(12, Associativity.Left, STAR, SLASH, PERCENT);
		RegisterOperators(13, Associativity.Right, PLUS_PLUS, MINUS_MINUS, TILDE, EMARK, NEW);
		RegisterOperators(14, Associativity.Left, DOT);
		RegisterOperators(15, Associativity.Neutral, R_PAR, R_BKT);
		#endregion
		#endregion

		Root = compilation_unit;

		mSkipTokensInPreview.UnionWith(new Terminal[] { identifier_raw, DOT_RAW, COMMA, L_BKT_RAW, R_BKT, QMARK });
		MarkTransient(
		#region Transients
		type_parameters_opt,
		  super_opt,
		  interfaces_opt,
		  type_arguments_opt,
		  type_bound_opt,
		  type_declaration,
		  import_declaration,
		  modifier,
		  modifiers_opt,
		  enum_body_declarations_opt,
		  expression,
		  L_BKT,
		  L_PAR,
		  LT
		#endregion
		);
	}


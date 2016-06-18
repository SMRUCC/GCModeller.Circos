Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Colors
Imports Microsoft.VisualBasic.Linq.Extensions
Imports Microsoft.VisualBasic.ComponentModel.DataStructures

Namespace Documents.Karyotype.Highlights

    Public Class GradientMappings : Inherits Highlights

        Sub New(locis As IEnumerable(Of Loci.Abstract.ILoci),
                length As Integer,
                mapName As String,
                winSize As Integer,
                Optional replaceBase As Boolean = False,
                Optional extTails As Boolean = False)
            Dim d = (From site As Loci.Abstract.ILoci
                     In locis
                     Select site
                     Group site By site.Left Into Group).ToDictionary(Function(site) site.Left,
                                                                      Function(site) CDbl(site.Group.ToArray.Length))
            Call __initCommon(d, length, mapName, winSize, replaceBase, extTails)
        End Sub

        Protected Sub __initCommon(d As Dictionary(Of Integer, Double),
                                   length As Integer,
                                   mapName As String,
                                   winSize As Integer,
                                   replaceBase As Boolean,
                                   extTails As Boolean)
            Dim values = length.ToArray(Function(idx) d.TryGetValue(idx, [default]:=0))
            Dim avgs As Double()

            Call $"  >>{Me.GetType.FullName}   min= {values.Min};   max={values.Max};  @{mapName}".__DEBUG_ECHO

            If winSize > 0 Then
                Dim slids = values.CreateSlideWindows(winSize, extTails:=extTails)  '划窗平均值
                avgs = slids.ToArray(Function(win, idx) win.Average) '- values(idx) / 15)
            Else
                avgs = values
            End If

            Dim colors As Mappings() = GradientMaps.GradientMappings(avgs, mapName, replaceBase:=replaceBase)
            Me._highLights = colors.ToArray(Function(site, idx) FromColorMapping(site, idx + 1, 0))
        End Sub

        Sub New(values As IEnumerable(Of Double),
                length As Integer,
                mapName As String,
                winSize As Integer,
                Optional replaceBase As Boolean = False,
                Optional extTails As Boolean = False)
            Dim d As Dictionary(Of Integer, Double) =
                values.Sequence.ToDictionary(Function(i) i, Function(i) values(i))
            Call __initCommon(d, length, mapName, winSize, replaceBase, extTails)
        End Sub
    End Class
End Namespace
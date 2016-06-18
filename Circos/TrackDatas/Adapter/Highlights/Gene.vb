Imports System.Drawing
Imports System.Text
Imports System.Text.RegularExpressions
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Colors
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports LANS.SystemsBiology.ComponentModel
Imports LANS.SystemsBiology.GCModeller.DataVisualization
Imports Microsoft.VisualBasic.Imaging

Namespace TrackDatas.Highlights

    ''' <summary>
    ''' 使用highlights来标记基因组之中的基因
    ''' </summary>
    Public Class GeneMark : Inherits Highlights

        Dim COGColors As Dictionary(Of String, String)

        Sub New(annos As IEnumerable(Of IGeneBrief), Color As Dictionary(Of String, String))
            Me._highLights = (From GeneObject As IGeneBrief
                              In annos
                              Let COG As String = If(String.IsNullOrEmpty(GeneObject.COG), "-", GeneObject.COG)
                              Let attr As RegionTrackData = New RegionTrackData With {
                                  .start = CInt(GeneObject.Location.Left),
                                  .end = CInt(GeneObject.Location.Right),
                                  .formatting = New Formatting With {.fill_color = If(Color.ContainsKey(COG), Color(COG), CircosColor.DefaultCOGColor)}
                              }
                              Select attr).ToArray
            Me.COGColors = Color
        End Sub

        Public Function LegendsDrawing(ref As Point, ByRef gdi As GDIPlusDeviceHandle) As Point
            Dim COGColors = (From clProfile In Me.COGColors
                             Select clProfile.Key,
                                 Cl = CircosColor.FromKnownColorName(clProfile.Value)) _
                                    .ToDictionary(Function(cl) cl.Key,
                                                  Function(x) DirectCast(New SolidBrush(x.Cl), Brush))
            Dim Margin As Integer = 50
            Dim font As New Font(FontFace.MicrosoftYaHei, 20)
            Dim w As Integer = CInt(gdi.Width * 0.3)

            ref = New Point(Margin, gdi.Height - 7 * Margin)

            Call gdi.Graphics.DrawingCOGColors(COGColors, ref, font, w, Margin)

            Return ref
        End Function
    End Class
End Namespace
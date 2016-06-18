﻿Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.Configurations.Nodes.Plots
Imports LANS.SystemsBiology.AnalysisTools.DataVisualization.Interaction.Circos.TrackDatas
Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Configurations.Nodes.Plots

    ''' <summary>
    ''' Links are defined in ``&lt;link>`` blocks enclosed in a ``&lt;links>`` block. 
    ''' The links start at a radial position defined by 'radius' and have their
    ''' control point (adjusts curvature) at the radial position defined by
    ''' 'bezier_radius'. In this example, I use the segmental duplication
    ''' data Set, which connects regions Of similar sequence (90%+
    ''' similarity, at least 1kb In size).
    ''' </summary>
    Public Class Links

    End Class

    Public Class link : Inherits TracksPlot(Of TrackDatas.link)

        Public Overrides ReadOnly Property type As String
            Get
                Return "link"
            End Get
        End Property

        Public Property radius As String = "0.8r"
        Public Property bezier_radius As String = "0r"
        Public Property color As String = "black_a4"

        Sub New(data As data(Of TrackDatas.link))
            Call MyBase.New(data)
        End Sub

        Protected Overrides Function GetProperties() As String()
            Return SimpleConfig.GenerateConfigurations(Of link)(Me)
        End Function
    End Class
End Namespace
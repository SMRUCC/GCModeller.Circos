﻿#Region "Microsoft.VisualBasic::46c682bb7521482fb3e51d77a76e355c, ..\interops\visualize\Circos\Circos\ConfFiles\Nodes\Line\Nodes.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Settings

Namespace Configurations.Nodes.Plots.Lines

    Public Class Axis : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property color As String = "lgrey_a2"
        <Circos> Public Property thickness As String = "1"
        <Circos> Public Property spacing As String = "0.025r"

        Public Overrides Function Build(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("axis", IndentLevel, Nothing)
        End Function
    End Class

    Public Class Background : Inherits CircosDocument
        Implements ICircosDocNode

        <Circos> Public Property color As String = "vvlred"
        <Circos> Public Property y1 As String = "0.002"
        <Circos> Public Property y0 As String = "0.006"

        Public Overrides Function Build(IndentLevel As Integer) As String
            Return Me.GenerateCircosDocumentElement("background", IndentLevel, Nothing)
        End Function
    End Class
End Namespace

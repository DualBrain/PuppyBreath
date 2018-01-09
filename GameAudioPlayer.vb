﻿'Add Project References to:
'   PresentationCore
'   WindowsBase

Imports System.Windows.Media

''' <summary>
''' Represents an instance of System.Windows.Media.MediaPlayer used by GameAudio to play a sound effect or music track.
''' </summary>
''' <author>Reed Kimble 01/09/2018</author>
Public Class GameAudioPlayer
    Inherits MediaPlayer

    ''' <summary>
    ''' Gets a value that indicates if the player is actively playing sound.
    ''' </summary>
    ''' <returns>True if the player is playing, otherwise false.</returns>
    Public ReadOnly Property IsPlaying As Boolean

    Private isLooping As Boolean
    Private lastPlayedResource As Uri

    Public Sub New()
        AddHandler Me.MediaEnded, AddressOf OnMediaEnded
        AddHandler Me.MediaOpened, AddressOf OnMediaOpened
    End Sub

    ''' <summary>
    ''' Play the resource specified as a sound effect (one-shot).
    ''' </summary>
    ''' <param name="resource">A Uri pointing to the WAV file in the Resources folder.</param>
    Public Shadows Sub Play(resource As Uri)
        If lastPlayedResource IsNot Nothing AndAlso lastPlayedResource = resource Then
            Position = TimeSpan.Zero
            MyBase.Play()
        Else
            Open(resource)
            MyBase.Play()
        End If
    End Sub

    ''' <summary>
    ''' Play the resource specified as a looping music track (loop-until-stopped).
    ''' </summary>
    ''' <param name="resource">A Uri pointing to the WAV file in the Resources folder.</param>
    Public Sub PlayLooping(resource As Uri)
        isLooping = True
        Play(resource)
    End Sub

    ''' <summary>
    ''' Resumes playing of a paused sound.
    ''' </summary>
    Public Sub [Resume]()
        MyBase.Play()
    End Sub

    ''' <summary>
    ''' Stops playing a looping music track. The standard Stop() method will restart a looping music track.
    ''' Use StopLoop() to end a looping music track.
    ''' </summary>
    Public Sub StopLoop()
        isLooping = False
        [Stop]()
    End Sub

    Protected Sub OnMediaEnded(sender As Object, e As EventArgs)
        If isLooping Then
            Position = TimeSpan.Zero
            MyBase.Play()
            Exit Sub
        End If
        _IsPlaying = False
        Close()
    End Sub

    Protected Sub OnMediaOpened(sender As Object, e As EventArgs)
        _IsPlaying = True
    End Sub
End Class
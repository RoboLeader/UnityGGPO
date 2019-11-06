﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using UnityEngine;

public static partial class GGPO {

    public static class Session {
        // Pass throughs

        public delegate bool OnEventConnectedToPeerDelegate(int connected_player);

        public delegate bool OnEventSynchronizingWithPeerDelegate(int synchronizing_player, int synchronizing_count, int synchronizing_total);

        public delegate bool OnEventSynchronizedWithPeerDelegate(int synchronizing_player);

        public delegate bool OnEventRunningDelegate();

        public delegate bool OnEventConnectionInterruptedDelegate(int connection_interrupted_player, int connection_interrupted_disconnect_timeout);

        public delegate bool OnEventConnectionResumedDelegate(int connection_resumed_player);

        public delegate bool OnEventDisconnectedFromPeerDelegate(int disconnected_player);

        public delegate bool OnEventEventcodeTimesyncDelegate(int timesync_frames_ahead);

        public delegate bool SafeLoadGameStateDelegate(NativeArray<byte> data);

        public delegate bool SafeLogGameStateDelegate(string filename, NativeArray<byte> data);

        public delegate NativeArray<byte> SafeSaveGameStateDelegate(out int checksum, int frame);

        public delegate void SafeFreeBufferDelegate(NativeArray<byte> data);

        static IntPtr ggpo;
        static Action<string> logDelegate;
        static readonly Dictionary<long, NativeArray<byte>> cache = new Dictionary<long, NativeArray<byte>>();

        static SafeLoadGameStateDelegate loadGameStateCallback;
        static SafeLogGameStateDelegate logGameStateCallback;
        static SafeSaveGameStateDelegate saveGameStateCallback;

        static SafeFreeBufferDelegate freeBufferCallback;

        static OnEventConnectedToPeerDelegate onEventConnectedToPeer;
        static OnEventSynchronizingWithPeerDelegate onEventSynchronizingWithPeer;
        static OnEventSynchronizedWithPeerDelegate onEventSynchronizedWithPeer;
        static OnEventRunningDelegate onEventRunning;
        static OnEventConnectionInterruptedDelegate onEventConnectionInterrupted;
        static OnEventConnectionResumedDelegate onEventConnectionResumed;
        static OnEventDisconnectedFromPeerDelegate onEventDisconnectedFromPeer;
        static OnEventEventcodeTimesyncDelegate onEventTimesync;

        static IntPtr _beginGame;
        static IntPtr _advanceFrame;
        static IntPtr _loadGameStateCallback;
        static IntPtr _logGameStateCallback;
        static IntPtr _saveGameStateCallback;
        static IntPtr _freeBufferCallback;
        static IntPtr _onEventCallback;

        public static void Init(Action<string> log) {
            logDelegate = log;
        }

        public static bool IsStarted() {
            return ggpo != IntPtr.Zero;
        }

        public static void StartSession(
                BeginGameDelegate beginGame,
                AdvanceFrameDelegate advanceFrame,
                SafeLoadGameStateDelegate loadGameState,
                SafeLogGameStateDelegate logGameState,
                SafeSaveGameStateDelegate saveGameState,
                SafeFreeBufferDelegate freeBuffer,
                OnEventConnectedToPeerDelegate onEventConnectedToPeer,
                OnEventSynchronizingWithPeerDelegate onEventSynchronizingWithPeer,
                OnEventSynchronizedWithPeerDelegate onEventSynchronizedWithPeer,
                OnEventRunningDelegate onEventRunning,
                OnEventConnectionInterruptedDelegate onEventConnectionInterrupted,
                OnEventConnectionResumedDelegate onEventConnectionResumed,
                OnEventDisconnectedFromPeerDelegate onEventDisconnectedFromPeer,
                OnEventEventcodeTimesyncDelegate onEventTimesync,
                string gameName, int numPlayers, int localport) {
            loadGameStateCallback = loadGameState;
            logGameStateCallback = logGameState;
            saveGameStateCallback = saveGameState;
            freeBufferCallback = freeBuffer;

            Session.onEventConnectedToPeer = onEventConnectedToPeer;
            Session.onEventSynchronizingWithPeer = onEventSynchronizingWithPeer;
            Session.onEventSynchronizedWithPeer = onEventSynchronizedWithPeer;
            Session.onEventRunning = onEventRunning;
            Session.onEventConnectionInterrupted = onEventConnectionInterrupted;
            Session.onEventConnectionResumed = onEventConnectionResumed;
            Session.onEventDisconnectedFromPeer = onEventDisconnectedFromPeer;
            Session.onEventTimesync = onEventTimesync;

            unsafe {
                _beginGame = Marshal.GetFunctionPointerForDelegate(beginGame);
                _advanceFrame = Marshal.GetFunctionPointerForDelegate(advanceFrame);
                _loadGameStateCallback = Marshal.GetFunctionPointerForDelegate<LoadGameStateDelegate>(LoadGameState);
                _logGameStateCallback = Marshal.GetFunctionPointerForDelegate<LogGameStateDelegate>(LogGameState);
                _saveGameStateCallback = Marshal.GetFunctionPointerForDelegate<SaveGameStateDelegate>(SaveGameState);
                _freeBufferCallback = Marshal.GetFunctionPointerForDelegate<FreeBufferDelegate>(FreeBuffer);
                _onEventCallback = Marshal.GetFunctionPointerForDelegate<OnEventDelegate>(OnEventCallback);
            }
            GGPO.StartSession(out ggpo,
                _beginGame,
                _advanceFrame,
                _loadGameStateCallback,
                _logGameStateCallback,
                _saveGameStateCallback,
                _freeBufferCallback,
                _onEventCallback,
                gameName, numPlayers, localport);

            Debug.Assert(ggpo != IntPtr.Zero);
        }

        public static void StartSpectating(
                BeginGameDelegate beginGame,
                AdvanceFrameDelegate advanceFrame,
                SafeLoadGameStateDelegate loadGameState,
                SafeLogGameStateDelegate logGameState,
                SafeSaveGameStateDelegate saveGameState,
                SafeFreeBufferDelegate freeBuffer,
                OnEventConnectedToPeerDelegate onEventConnectedToPeer,
                OnEventSynchronizingWithPeerDelegate onEventSynchronizingWithPeer,
                OnEventSynchronizedWithPeerDelegate onEventSynchronizedWithPeer,
                OnEventRunningDelegate onEventRunning,
                OnEventConnectionInterruptedDelegate onEventConnectionInterrupted,
                OnEventConnectionResumedDelegate onEventConnectionResumed,
                OnEventDisconnectedFromPeerDelegate onEventDisconnectedFromPeer,
                OnEventEventcodeTimesyncDelegate onEventTimesync,
                string gameName, int numPlayers, int localport, string hostIp, int hostPort) {
            loadGameStateCallback = loadGameState;
            logGameStateCallback = logGameState;
            saveGameStateCallback = saveGameState;
            freeBufferCallback = freeBuffer;

            Session.onEventConnectedToPeer = onEventConnectedToPeer;
            Session.onEventSynchronizingWithPeer = onEventSynchronizingWithPeer;
            Session.onEventSynchronizedWithPeer = onEventSynchronizedWithPeer;
            Session.onEventRunning = onEventRunning;
            Session.onEventConnectionInterrupted = onEventConnectionInterrupted;
            Session.onEventConnectionResumed = onEventConnectionResumed;
            Session.onEventDisconnectedFromPeer = onEventDisconnectedFromPeer;
            Session.onEventTimesync = onEventTimesync;

            unsafe {
                _beginGame = Marshal.GetFunctionPointerForDelegate(beginGame);
                _advanceFrame = Marshal.GetFunctionPointerForDelegate(advanceFrame);
                _loadGameStateCallback = Marshal.GetFunctionPointerForDelegate<LoadGameStateDelegate>(LoadGameState);
                _logGameStateCallback = Marshal.GetFunctionPointerForDelegate<LogGameStateDelegate>(LogGameState);
                _saveGameStateCallback = Marshal.GetFunctionPointerForDelegate<SaveGameStateDelegate>(SaveGameState);
                _freeBufferCallback = Marshal.GetFunctionPointerForDelegate<FreeBufferDelegate>(FreeBuffer);
                _onEventCallback = Marshal.GetFunctionPointerForDelegate<OnEventDelegate>(OnEventCallback);
            }

            GGPO.StartSpectating(out ggpo, 
                _beginGame,
                _advanceFrame,
                _loadGameStateCallback,
                _logGameStateCallback,
                _saveGameStateCallback,
                _freeBufferCallback,
                _onEventCallback,
                gameName, numPlayers, localport, hostIp, hostPort);
        }

        public static int GetNetworkStats(int phandle, out GGPONetworkStats stats) {
            stats = new GGPONetworkStats();
            var result = GGPO.GetNetworkStats(ggpo, phandle,
                out stats.send_queue_len,
                out stats.recv_queue_len,
                out stats.ping,
                out stats.kbps_sent,
                out stats.local_frames_behind,
                out stats.remote_frames_behind
            );
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int SetDisconnectNotifyStart(int timeout) {
            var result = GGPO.SetDisconnectNotifyStart(ggpo, timeout);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int SetDisconnectTimeout(int timeout) {
            var result = GGPO.SetDisconnectTimeout(ggpo, timeout);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int SynchronizeInput(ulong[] inputs, int length, out int disconnect_flags) {
            var result = GGPO.SynchronizeInput(ggpo, inputs, length, out disconnect_flags);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int AddLocalInput(int local_player_handle, ulong inputs) {
            var result = GGPO.AddLocalInput(ggpo, local_player_handle, inputs);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int CloseSession() {
            foreach (var data in cache.Values) {
                freeBufferCallback(data);
            }
            cache.Clear();
            var result = GGPO.CloseSession(ggpo);
            Debug.Assert(SUCCEEDED(result));
            ggpo = IntPtr.Zero;
            return result;
        }

        public static int Idle(int time) {
            var result = GGPO.Idle(ggpo, time);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int AddPlayer(GGPOPlayer player, out int phandle) {
            var result = GGPO.AddPlayer(ggpo,
                (int)player.type,
                player.player_num,
                player.ip_address,
                player.port,
                out phandle);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int DisconnectPlayer(int phandle) {
            var result = GGPO.DisconnectPlayer(ggpo, phandle);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int SetFrameDelay(int phandle, int frame_delay) {
            var result = GGPO.SetFrameDelay(ggpo, phandle, frame_delay);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static int AdvanceFrame() {
            var result = GGPO.AdvanceFrame(ggpo);
            Debug.Assert(SUCCEEDED(result));
            return result;
        }

        public static void Log(string v) {
            GGPO.Log(ggpo, v);
        }

        public static void OnDispose() {
            if (ggpo != IntPtr.Zero) {
                CloseSession();
            }
        }

        // Callbacks

        static unsafe void FreeBuffer(void* dataPtr) {
            if (cache.TryGetValue((long)dataPtr, out var data)) {
                freeBufferCallback(data);
                cache.Remove((long)dataPtr);
            }
        }

        static unsafe bool SaveGameState(void** buffer, int* outLen, int* outChecksum, int frame) {
            var data = saveGameStateCallback(out int checksum, frame);
            var ptr = Helper.ToPtr(data);
            cache[(long)ptr] = data;

            *buffer = ptr;
            *outLen = data.Length;
            *outChecksum = checksum;
            return true;
        }

        static unsafe bool LogGameState(string filename, void* buffer, int length) {
            return logGameStateCallback(filename, Helper.ToArray(buffer, length));
        }

        static unsafe bool LoadGameState(void* buffer, int length) {
            return loadGameStateCallback(Helper.ToArray(buffer, length));
        }

        static bool OnEventCallback(IntPtr evtPtr) {
            /*
            code = data[0];
            connected.player = data[1];
            synchronizing.player = data[1];
            synchronizing.count = data[2];
            synchronizing.total = data[3];
            synchronized.player = data[1];
            disconnected.player = data[1]
            timesync.frames_ahead = data[1];
            connection_interrupted.player = data[1];
            connection_interrupted.disconnect_timeout = data[2];
            connection_resumed.player = data[1];
            */

            int[] data = new int[4];
            Marshal.Copy(evtPtr, data, 0, 4);
            switch (data[0]) {
                case EVENTCODE_CONNECTED_TO_PEER:
                    return onEventConnectedToPeer(data[1]);

                case EVENTCODE_SYNCHRONIZING_WITH_PEER:
                    return onEventSynchronizingWithPeer(data[1], data[2], data[3]);

                case EVENTCODE_SYNCHRONIZED_WITH_PEER:
                    return onEventSynchronizedWithPeer(data[1]);

                case EVENTCODE_RUNNING:
                    return onEventRunning();

                case EVENTCODE_DISCONNECTED_FROM_PEER:
                    return onEventDisconnectedFromPeer(data[1]);

                case EVENTCODE_TIMESYNC:
                    return onEventTimesync(data[1]);

                case EVENTCODE_CONNECTION_INTERRUPTED:
                    return onEventConnectionInterrupted(data[1], data[2]);

                case EVENTCODE_CONNECTION_RESUMED:
                    return onEventConnectionResumed(data[1]);
            }
            return false;
        }
    }
}

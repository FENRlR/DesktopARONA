package com.gdx.arona;

import com.badlogic.gdx.backends.lwjgl3.Lwjgl3Application;
import com.badlogic.gdx.backends.lwjgl3.Lwjgl3ApplicationConfiguration;
import com.badlogic.gdx.graphics.Color;
import com.gdx.arona.arona;
import com.gdx.arona.audiosync.AudioSampler;
import com.gdx.arona.audiosync.speaker;

import javax.sound.sampled.LineUnavailableException;
import java.awt.*;

public class DesktopLauncher {
	public static void main (String[] arg) {
		telemetry telemetry = new telemetry();
		Thread tel = new Thread(telemetry);
		tel.start();
		
		audiocable audiocable = new audiocable();
		Thread ado = new Thread(audiocable);
		ado.start();

		Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
		int wd = screenSize.width;
		int ht = screenSize.height;

		Lwjgl3ApplicationConfiguration config = new Lwjgl3ApplicationConfiguration();
		config.setForegroundFPS(60);
		config.setTitle("Arona");
		config.setWindowIcon("./aronares/icon2.png");
		config.setDecorated(false);
		config.setTransparentFramebuffer(true);
		config.setWindowedMode(495,ht-110);
		config.setWindowPosition((int)(wd/1.5)+135,70);

		Lwjgl3Application arona_exe = new Lwjgl3Application(new arona(), config);
	}
}

package com.gdx.arona;

import com.badlogic.gdx.ApplicationAdapter;
import com.badlogic.gdx.Input;
import com.badlogic.gdx.graphics.*;
import com.badlogic.gdx.graphics.Color;
import com.badlogic.gdx.graphics.g2d.*;
import com.badlogic.gdx.graphics.glutils.FrameBuffer;
import com.badlogic.gdx.math.Vector3;
import com.badlogic.gdx.scenes.scene2d.Stage;
import com.badlogic.gdx.utils.Align;
import com.badlogic.gdx.utils.Array;
import com.badlogic.gdx.utils.ScreenUtils;
import com.badlogic.gdx.Gdx;
import com.badlogic.gdx.utils.viewport.ScreenViewport;
import com.esotericsoftware.spine.*;
import com.esotericsoftware.spine.Animation;
import com.esotericsoftware.spine.Event;
import com.esotericsoftware.spine.SkeletonBinary;
import com.esotericsoftware.spine.utils.TwoColorPolygonBatch;
import com.esotericsoftware.spine.AnimationState.AnimationStateListener;
import com.esotericsoftware.spine.AnimationState.TrackEntry;
import javax.imageio.ImageIO;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.Random;
import static com.badlogic.gdx.graphics.GL20.*;
import static com.badlogic.gdx.graphics.GL30.GL_SRGB;
import com.badlogic.gdx.scenes.scene2d.ui.TextField;
import com.badlogic.gdx.graphics.g2d.freetype.FreeTypeFontGenerator;
import com.badlogic.gdx.scenes.scene2d.ui.Label;
import com.gdx.arona.spine_pma_fix.src.com.esotericsoftware.spine.SkeletonRenderer;

public class arona extends ApplicationAdapter {
	PolygonSpriteBatch batch;
	SpriteBatch bach;
	Texture img;
	Texture tbg;
	OrthographicCamera camera;
	SkeletonRenderer renderer;
	SkeletonRendererDebug debugRenderer;
	TextureAtlas atlas;
	Skeleton skeleton;
	AnimationState state;
	Animation rani;
	Animation pat;
	Image screenShot;
	Texture bgtex;
	FrameBuffer fbo;
	TextureRegion fboRegion;
	boolean drawFbo = true;
	Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
	int width = screenSize.width;
	int height= screenSize.height;

	int abskx = 250;
	int absky = 355;
	
	float facex = abskx+8;
	float facey = absky+392;

	Vector3 mousePos = new Vector3();
	float relx;
	float rely;
	float degree;
	float unit=360;
	float offset = (float)(3.33*0.24);
	float actualdur = (float)(3.33*0.61);
	float meta;
	
	boolean ranisw = false;
	int motsw = 0;
	public static int txton = 0;
	String inputmsg="";

	submod sb = new submod();

	private Stage txtmod;

	BitmapFont textfont;
	FreeTypeFontGenerator generator;
	FreeTypeFontGenerator.FreeTypeFontParameter parameter;
	public static String ctxt = "";
	public static String rtxt = "";
	TextField textbox;
	TextField.TextFieldStyle dff = new TextField.TextFieldStyle();

	int textboxlock = 0;
	public static int msgontherocks = 0;

	public int eyeclose = 0;
	public int eyecount=0;
	Random rand = new Random();
	public int eyelimit = rand.nextInt(61) + 240;

	public int lipcount = 0;
	public static String lipvar = "";

	public int brcount = 0;
	public int brduration = 0;
	public int orgsm = 0;

	private int txtboxtime = 0;
	public static int ifboxnull = 0;

	//@Override
	public void create () {
		txtmod = new Stage(new ScreenViewport());
		Gdx.input.setInputProcessor(txtmod);
		bach = new SpriteBatch();
		tbg = new Texture("./aronares/emballon4.png");
		batch = new PolygonSpriteBatch();
		camera = new OrthographicCamera();
		renderer = new SkeletonRenderer();
		atlas = new TextureAtlas(Gdx.files.internal("./aronares/arona_spr.atlas"));
		SkeletonBinary skb = new SkeletonBinary(atlas);
		skb.setScale(0.45f);
		SkeletonData skeletonData = skb.readSkeletonData(Gdx.files.internal("./aronares/arona_spr.skel"));
		skeleton = new Skeleton(skeletonData);
		skeleton.setPosition(abskx, absky);
		AnimationStateData stateData = new AnimationStateData(skeletonData);

		for(int i=0; i<39; i++)
		{
			stateData.setMix(sb.motion(i), sb.motion(i), 0.2f);
		}
		stateData.setMix(sb.motion(37), sb.motion(39), 0.1f);
		String[] liplist = {"01","20","31","02"};
		for(int i=0; i<liplist.length; i++)
		{
			for(int j=0; j<liplist.length; j++)
			{
				if(!liplist[i].equals(liplist[j]))
				{
					stateData.setMix(liplist[i], liplist[j], 0.02f);
					stateData.setMix(liplist[j], liplist[i], 0.02f);
				}
			}
		}
		state = new AnimationState(stateData);
		state.setTimeScale(0.5f);
		state.setAnimation(0, "Idle_01", true);
		rani = skeletonData.findAnimation(sb.motion(35));
		pat = skeletonData.findAnimation(sb.motion(37));
		
		generator = new FreeTypeFontGenerator(Gdx.files.internal("./aronares/font/MainFont.ttf"));
		parameter = new FreeTypeFontGenerator.FreeTypeFontParameter();
		dff.fontColor = Color.BLACK;
	}

	@Override
	public void render () {
		if(Gdx.input.isButtonJustPressed(Input.Buttons.LEFT)&&motsw!=3){
			camera.unproject(mousePos.set(Gdx.input.getX(), Gdx.input.getY(), 0));
			if(motsw!=1 && orgsm==0 && mousePos.x>210&&mousePos.x<300&&mousePos.y>605&&mousePos.y<665){
				brcount++;
				state.update((float)(1.0));
				if(brcount==1)
				{
					state.setAnimation(4, sb.motion(16),false);
					brduration = rand.nextInt(61) + 120;
				}
				else if(brcount==2)
				{
					state.setAnimation(4, sb.motion(17),false);
					brduration = rand.nextInt(61) + 150;
				}
				else
				{
					state.setAnimation(4, sb.motion(19),false).setTrackTime(1.0f);
					brduration = rand.nextInt(61) + 180;
				}
			}
		}
		else if(brcount>0)
		{
			brduration--;
			if(brduration==0)
			{
				brcount=0;
				state.setEmptyAnimation(4,0);
			}
		}
		else if(Gdx.input.isButtonPressed(Input.Buttons.LEFT)){
			camera.unproject(mousePos.set(Gdx.input.getX(), Gdx.input.getY(), 0));
			relx = mousePos.x - facex;
			rely = mousePos.y - facey;
			if(mousePos.x>220&&mousePos.x<310&&mousePos.y>810&&mousePos.y<860&&motsw<2||motsw==1){
				motsw=1;
				state.setAnimation(3, sb.motion(37),false).setAnimationStart(lrgf(relx,(float)(0.5),(float)(0.0)));
			}
			else if(Math.sqrt(Math.pow(mousePos.x-248,2) + Math.pow(mousePos.y-459,2)) < 30 &&motsw!=1&&brcount==0||motsw==3){
				motsw=3;
				state.setAnimation(4, sb.motion(6),false);
				state.update((float)(1.0));
				orgsm = 1;
			}
			else if(Math.sqrt(Math.pow(relx,2) + Math.pow(rely,2)) < 113 &&motsw!=1){
				motsw = 2;
				rani.apply(skeleton, -1, 0, false, null, 1, Animation.MixBlend.first, Animation.MixDirection.in);
			}
			else if(motsw!=1 && Math.sqrt(Math.pow(relx,2) + Math.pow(rely,2)) > 113){
				motsw = 2;
				degree = (float) Math.toDegrees(Math.atan2(rely, relx)) + 180;// math.atan2(y, x);
				if(degree>360){
					degree-=180;
				}
				meta = (float)((degree)*(actualdur/unit)+offset);
				rani.apply(skeleton, -1, meta, false, null, 1, Animation.MixBlend.first, Animation.MixDirection.in);
			}
		}
		else if(motsw!=0){
			if(motsw==1)
			{
				state.addAnimation(3, sb.motion(36), false,0);
				state.setEmptyAnimation(3,0);
			}
			else if(motsw==2)
			{
				state.addAnimation(3, sb.motion(34), false,0);
				state.addEmptyAnimation(3,0,0);
			}
			else if(motsw==3){
				state.addEmptyAnimation(4,0,0);
				orgsm=0;
			}
			motsw = 0;
			eyecount = 0;
		}
		else{
			eyecount++;
			if (eyecount>=eyelimit)
			{
				if(eyeclose==0)
				{
					state.setAnimation(1, sb.motion(33),false);
					eyecount = 0;
					eyelimit = 10;
					eyeclose = 1;
				}
				else
				{
					state.setEmptyAnimation(1,0.5f);
					eyecount = 0;
					eyelimit = rand.nextInt(61) + 360;
					eyeclose = 0;
				}
			}
		}
		if(!aninull().equals(lipvar)&&lipvar!="")
		{
			state.setAnimation(2, lipvar,false);
			lipcount = 0;
		}
		else if(lipcount >= 12)
		{
			state.setAnimation(2, "01",false);
			state.setEmptyAnimation(2,0.03f);
			lipvar = "";
			lipcount = 0;
		}
		else if(aninull().equals(lipvar))
		{
			lipcount += 1;
		}

		if(Gdx.input.isKeyJustPressed(Input.Keys.ENTER)&& txton==0)
		{
			if(ctxt.equals("")&&txtmod.getActors().size>0)
			{
				txtmod.clear();
			}
			try {
				textboxinput tbox = new textboxinput();
			} catch (IOException e) {
				e.printStackTrace();
			}
			txton=1;
		}
		else if(txton==1)
		{
			if(msgontherocks==1)
			{
				if(!rtxt.equals(""))
				{
					parameter.size = 24;
					parameter.color.set(0.16f, 0.255f, 0.353f, 1);
					parameter.characters = rtxt;
					textfont = generator.generateFont(parameter);
					dff.font = textfont;

					com.badlogic.gdx.scenes.scene2d.ui.Skin labelskin = new com.badlogic.gdx.scenes.scene2d.ui.Skin();
					labelskin.add("back", tbg);
					Label.LabelStyle labelStyle = new Label.LabelStyle();
					labelStyle.font = textfont;
					labelStyle.background = labelskin.getDrawable("back");

					Label labeltext = new Label(rtxt,labelStyle);
					labeltext.setWrap(true);
					labeltext.setAlignment(Align.center);
					float lbscale = 0.81f;

					labeltext.setSize(tbg.getWidth()*lbscale,tbg.getHeight()*lbscale);
					labeltext.setPosition(0,260);
					txtmod.addActor(labeltext);
				}
				
				txton=0;
				rtxt = "";
				msgontherocks = 0;
			}
			else if(ifboxnull==1)
			{
				ifboxnull = 0;
				txton = 0;
			}
		}
		if(txtmod.getActors().size>0)
		{
			txtboxtime++;
			if(txtboxtime>720)
			{
				txtboxtime = 0;
				txtmod.clear();
				txton = 0;
			}
		}
		Gdx.gl.glClearColor(0, 0, 0, 0);
		Gdx.gl.glClear(GL20.GL_COLOR_BUFFER_BIT);
		state.update(Gdx.graphics.getDeltaTime());
		state.apply(skeleton);
		skeleton.updateWorldTransform();
		
		camera.update();
		batch.getProjectionMatrix().set(camera.combined);
		batch.begin();
		renderer.draw(batch, skeleton);
		batch.end();

		txtmod.act(Gdx.graphics.getDeltaTime());
		txtmod.draw();
	}
	
	@Override
	public void resize (int width, int height) {
		camera.setToOrtho(false);
	}

	@Override
	public void dispose () {
		img.dispose();
		bgtex.dispose();
		bach.dispose();
		batch.dispose();

	}

	@Override
	public void pause() {
		//TODO : reserve
	}

	@Override
	public void resume() {
		//TODO : reserve
	}

	private float lrgf(float x, float slope, float bias){
		return (float)(0.667*(0.23+0.52/(1+(Math.exp((double)(-slope*x-bias))))));
	}

	private String aninull(){
		try{
			return state.getCurrent(2).getAnimation().getName();
		}catch(Exception e){
			return "";
		}
	}
}

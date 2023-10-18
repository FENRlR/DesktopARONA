package com.gdx.arona;

import javax.imageio.ImageIO;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

public class textboxinput implements ActionListener{
    JTextField tf1,tf2,tf3;
    JButton b1,b2;
    JFrame f = new JFrame();
    JLabel lab = new JLabel(new ImageIcon(ImageIO.read(new File("./aronares/tdialog.png"))));
    Font customFont = null;
    
    JPanel panel = new JPanel(new BorderLayout()) {
        BufferedImage image;
        {
            try {
                image = ImageIO.read(new File("./aronares/tdialog.png"));
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        @Override
        public void paintComponent(Graphics g) {
            super.paintComponent(g);
            g.drawImage(image, 0, 0, null);
        }
    };
    
    textboxinput() throws IOException {
        Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
        int wd = screenSize.width;
        int ht = screenSize.height;
        panel.add(lab);
        
        tf1 = new JTextField();
        tf1.addActionListener(this);
        try {
            customFont = Font.createFont(Font.TRUETYPE_FONT, new File("./aronares/font/MainFont.ttf")).deriveFont(24f);
        } catch (FontFormatException e) {
            e.printStackTrace();
        }
        tf1.setFont(customFont);
        tf1.setSize(new Dimension(1116, 81));
        tf1.setHorizontalAlignment(JLabel.CENTER);
        tf1.setBorder(javax.swing.BorderFactory.createEmptyBorder());
        tf1.setLocation(0,0);
        tf1.setOpaque(false);
        lab.add(tf1);
        
        arona a = new arona();
        f.add(lab);
        f.setSize(1116,81);
        f.setLocation((int)(wd/2)-550,(int)(ht/2)-100);
        f.setUndecorated(true);
        f.setBackground(new Color(0, 0, 0, 0));
        f.pack();
        f.setVisible(true);
        f.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        f.toFront();
        f.requestFocus();
    }

    public void actionPerformed(ActionEvent e) {
        arona.ctxt = tf1.getText();
        if(arona.ctxt.equals(""))
        {
            arona.ifboxnull = 1;
        }
        f.dispose();
    }
}

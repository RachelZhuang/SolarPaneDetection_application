#coding=utf-8
import sys
import cv2 as cv

if __name__ == '__main__':
    path=sys.argv[1]     

    # 读取图像，支持 bmp、jpg、png、tiff 等常用格式
    img = cv.imread(path)
    # 创建窗口并显示图像
    cv.namedWindow("Image")
    cv.imshow("Image", img)
    cv.waitKey(0)
    # 释放窗口
    cv.destroyAllWindows()